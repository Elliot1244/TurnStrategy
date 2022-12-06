using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Destructible : MonoBehaviour
{
    [SerializeField] private Transform _destructibleDestroyedPrefab;

    public static event EventHandler OnAnyDestructed;

    private GridPosition _gridPosition;
    private void Start()
    {
       _gridPosition =  LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public void Damage()
    {
        Transform destructibleDestroyedTransform = Instantiate(_destructibleDestroyedPrefab, transform.position, transform.rotation);

        ApplyExplosionToChildren(destructibleDestroyedTransform, 150f, transform.position, 10f);

        Destroy(gameObject);

        OnAnyDestructed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
