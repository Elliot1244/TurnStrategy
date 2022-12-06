using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExplode;

    [SerializeField] int _moveSpeed;
    [SerializeField] int _damageRadius;
    [SerializeField] float _reachedTargetDistance;
    [SerializeField] private Transform _grenadeExplodePrefab;
    [SerializeField] private TrailRenderer _grenadeTrail;
    [SerializeField] private AnimationCurve _arcYAnimationCurve;

    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
    private float _totalDistance;
    private Vector3 _positionXZ;

    private void Update()
    {
        Vector3 moveDir = (targetPosition - _positionXZ).normalized;

        _positionXZ += moveDir * _moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(_positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / _totalDistance;

        float maxHeight = _totalDistance / 4f;

        float positionY = _arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);

        if (Vector3.Distance(_positionXZ, targetPosition) < _reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, _damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }

                if (collider.TryGetComponent<Destructible>(out Destructible destructible))
                {
                    destructible.Damage();
                }
            }

            OnAnyGrenadeExplode?.Invoke(this, EventArgs.Empty);

            _grenadeTrail.transform.parent = null;

            Instantiate(_grenadeExplodePrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            
            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        }
    }


    public void SetUp(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        _positionXZ = transform.position;
        _positionXZ.y = 0;

        _totalDistance = Vector3.Distance(_positionXZ, targetPosition);
    }

}
