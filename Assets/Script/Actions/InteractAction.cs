using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    [SerializeField] private int _maxInteractDistance;

    private void Update()
    {
        if(!_isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAction GetEnemyAction(GridPosition gridPosition)
    {
        return new EnemyAction
        {
            _gridPosition = gridPosition,
            _actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPosition = new List<GridPosition>();


        //On récupère la position sur la grille de l'unit
        GridPosition unitGridPosition = _unit.GetGridPosition();



        //On cherche depuis la gauche vers la droite sur l'axe x
        for (int x = -_maxInteractDistance; x <= _maxInteractDistance; x++)
        {
            for (int z = -_maxInteractDistance; z <= _maxInteractDistance; z++)
            {
                GridPosition offSetGridPosition = new GridPosition(x, z);

                //Opérateur + / - non existant pour une struct, voir GridPosition Ligne 52 à 60
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;


                //Si la position récupérée n'est pas valide sur la grille alors on continue l'itération de la boucle sans prendre en compte la valeur hors grille
                if (!LevelGrid.Instance.IsValidgridPosition(testGridPosition))
                {
                    continue;
                }

                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);

                //S'il n'y a pas d'object interactable sur la case, on ne la prend pas en compte
                if(interactable == null)
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
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        interactable.Interact(OnInteractComplete);
        ActionStart(onActionComplete);
    }

    private void OnInteractComplete()
    {
        Actioncomplete();
    }
}
