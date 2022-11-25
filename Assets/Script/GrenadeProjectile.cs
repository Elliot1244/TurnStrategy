using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] int _moveSpeed;
    [SerializeField] int _damageRadius;
    [SerializeField] float _reachedTargetDistance;

    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        transform.position += moveDir * _moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < _reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, _damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
            }

            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        }
    }


    public void SetUp(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }

}
