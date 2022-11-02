using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Transform _bulletHitVFXPrefab;

    private Vector3 _targetPosition;
    public void SetUp(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

        transform.position += moveDir * _bulletSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if(distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = _targetPosition;

            //On enlève le parent du trail rendrer
            _trailRenderer.transform.parent = null;

            Destroy(gameObject);

            //Au moment de la destruction du projectile, on invoque l'effet de particule
            Instantiate(_bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
        }
    }
}
