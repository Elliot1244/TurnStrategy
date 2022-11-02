using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }


    [SerializeField] int _maxShootDistance;
    [SerializeField] int _rotateToShootSpeed;
    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;
    private bool _canShootBullet;

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        _stateTimer -= Time.deltaTime;

        switch(_state)
        {
            //Lors de la visée, l'unit se tourne vers la _targetUnit
            case State.Aiming:
                Vector3 aimDir = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * _rotateToShootSpeed);
                break;
            case State.Shooting:
                if(_canShootBullet)
                {
                    Shoot();
                    _canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if(_stateTimer <= 0)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                    _state = State.Shooting;
                    float _shootingStateTimer = 0.1f;
                    _stateTimer = _shootingStateTimer;
                break;
            case State.Shooting:
                    _state = State.Cooloff;
                    float _coolOfftateTimer = 0.5f;
                    _stateTimer = _coolOfftateTimer;
                break;
            case State.Cooloff:
                Actioncomplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPosition = new List<GridPosition>();


        //On récupère la position sur la grille de l'unit
        GridPosition unitGridPosition = _unit.GetGridPosition();

        //On cherche depuis la gauche vers la droite sur l'axe x
        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
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
                if(_testDistance > _maxShootDistance)
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
                if(targetUnit.IsEnemy() == _unit.IsEnemy())
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
        ActionStart(onActionComplete);

        //On récupère la case de l'unit que l'on vise
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);



        _state = State.Aiming;
        float aimingStateTime = 1f;
        _stateTimer = aimingStateTime;

        _canShootBullet = true;
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = _targetUnit,
            shootingUnit = _unit
        }) ;
        _targetUnit.Damage(40);
    }
}
