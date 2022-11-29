using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{


    [SerializeField] int _maxThrowDistance;
    [SerializeField] private Transform _grenadePrefab;
    [SerializeField] private LayerMask _obstaclesLayerMask;


    private void Update()
    {
        if(!_isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAction GetEnemyAction(GridPosition gridPosition)
    {
        return new EnemyAction
            {
            _gridPosition = gridPosition,
            _actionValue = 0,
            };
    }

    private bool ObstacleInTheWay(GridPosition unitGridPosition, GridPosition targetGridPosition)
    {
        // Get the unit world position
        var unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
        // Get the target world position
        var targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        // Get the direction to the target
        var directionToTarget = (targetWorldPosition - unitWorldPosition).normalized;
        // Get the distance to the target
        var distanceToTarget = Vector3.Distance(unitWorldPosition, targetWorldPosition);
        // Create the ray - with height offset
        var offset = Vector3.up * 1f; 
        var ray = new Ray(unitWorldPosition + offset, directionToTarget);
        // Check if there is an obstacle in the way and return
        return Physics.Raycast(ray, distanceToTarget, _obstaclesLayerMask);
    }



    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPosition = new List<GridPosition>();


        //On récupère la position sur la grille de l'unit
        GridPosition unitGridPosition = _unit.GetGridPosition();

        

        //On cherche depuis la gauche vers la droite sur l'axe x
        for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
        {
            for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++)
            {
                GridPosition offSetGridPosition = new GridPosition(x, z);

                //Opérateur + / - non existant pour une struct, voir GridPosition Ligne 52 à 60
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;


                //Si la position récupérée n'est pas valide sur la grille alors on continue l'itération de la boucle sans prendre en compte la valeur hors grille
                if (!LevelGrid.Instance.IsValidgridPosition(testGridPosition))
                {
                    continue;
                }

                //Si la distance de test est supérieure à la distance max de shoot possible, on ne prend pas en compte la case
                int _testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (_testDistance > _maxThrowDistance)
                {
                    continue;
                }


                if (ObstacleInTheWay(unitGridPosition, testGridPosition))
                {
                    continue;
                }

                validGridPosition.Add(testGridPosition);
            }
        }

        return validGridPosition;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = Instantiate(_grenadePrefab, _unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.SetUp(gridPosition, onGrenadeBehaviourComplete);
        ActionStart(onActionComplete);
    }

    private void onGrenadeBehaviourComplete()
    {
        Actioncomplete();
    }
}
