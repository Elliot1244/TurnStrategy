using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    [SerializeField] private int _maxSwordDistance;
    [SerializeField] private int _rotateToShootSpeed;

    private State _state;

    private float _stateTimer;

    private Unit _targetUnit;

    public static event EventHandler OnAnySwordHit;

    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;

    private enum State
    {
        SwordBeforeHit,
        SwordAfterHit,
    }

    private void Update()
    {
        if(!_isActive)
        {
            return;
        }

        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            //Lors de la visée, l'unit se tourne vers la _targetUnit
            case State.SwordBeforeHit:
                Vector3 aimDir = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * _rotateToShootSpeed);
                break;
            case State.SwordAfterHit:

                break;
        }

        if (_stateTimer <= 0)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (_state)
        {
            case State.SwordBeforeHit:
                _state = State.SwordAfterHit;
                float _afterHitStateTimer = 0.6f;
                _stateTimer = _afterHitStateTimer;
                _targetUnit.Damage(85);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;

            case State.SwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                Actioncomplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAction GetEnemyAction(GridPosition gridPosition)
    {
        return new EnemyAction
        {
            _gridPosition = gridPosition,
            _actionValue = 200,
        };
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPosition = new List<GridPosition>();


        //On récupère la position sur la grille de l'unit
        GridPosition unitGridPosition = _unit.GetGridPosition();



        //On cherche depuis la gauche vers la droite sur l'axe x
        for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++)
        {
            for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++)
            {
                GridPosition offSetGridPosition = new GridPosition(x, z);

                //Opérateur + / - non existant pour une struct, voir GridPosition Ligne 52 à 60
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;


                //Si la position récupérée n'est pas valide sur la grille alors on continue l'itération de la boucle sans prendre en compte la valeur hors grille
                if (!LevelGrid.Instance.IsValidgridPosition(testGridPosition))
                {
                    continue;
                }

                //Si la position sur la grille n'est pas occupée par une autre unit, on ne prend pas en compte cette position
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);


                //Si les deux units sont dans la même équipe on ne les prend pas en compte
                if (targetUnit.IsEnemy() == _unit.IsEnemy())
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
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        _state = State.SwordBeforeHit;
        float _beforeHitStateTimer = 0.8f;
        _stateTimer = _beforeHitStateTimer;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public int GetMaxSwordDistance()
    { 
        return _maxSwordDistance; 
    }
}
