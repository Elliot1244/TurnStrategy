using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State _state;
    private float _timer;

    private void Awake()
    {
        _state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch(_state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    
                    if(TryTakeEnemyAction(SetStateTakingTurn))
                    {
                        _state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        _timer = 0.5f;
        _state = State.TakingTurn;
    }


    //On met le timer à deux secondes à chaque tour de l'ennemi qui prend le state TakingTurn
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            _state=State.TakingTurn;
            _timer = 2f;
        }
    }

    private bool TryTakeEnemyAction(Action onEnemyActionCmplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAction(enemyUnit, onEnemyActionCmplete))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAction(Unit enemyUnit, Action onEnemyActionComplete)
    {
        EnemyAction bestEnemyAction = null;
        BaseAction bestBaseAction = null;
        foreach(BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if(!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                //Si l'enemi ne peut pas dépenser de point d'action pour faire une action
                continue;
            }

            if(bestEnemyAction == null)
            {
                bestEnemyAction = baseAction.GetBestEnemyAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAction testEnemyAction = baseAction.GetBestEnemyAction();
                if(testEnemyAction != null && testEnemyAction._actionValue > bestEnemyAction._actionValue)
                {
                    bestEnemyAction = testEnemyAction;
                    bestBaseAction = baseAction;
                }
            }

            if (bestEnemyAction._actionValue == 0)
            {
                TurnSystem.Instance.NextTurn();
            }

            baseAction.GetBestEnemyAction();
        }

        if(bestEnemyAction != null && enemyUnit.TrySpendActionPoints(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAction._gridPosition, onEnemyActionComplete);
            return true;
        }
        else
        {
            return false;
        }

        
    }
}
