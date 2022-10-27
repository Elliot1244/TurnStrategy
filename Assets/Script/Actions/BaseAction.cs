using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public  abstract class BaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected bool _isActive;

    //Fontion delegete pour que spin() puisse la recevoir comme param�tre afin qu'elle s'execute puis execute automatiquement la fonction mise en param�tre
    //Le type Action (avec using System) remplace "delegate void".
    protected Action _onActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();


    //La fonction g�n�rique" qui permet de faire une action, elle sera appel�e et overrid�e dans chaque script qui fait une action
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPosition();
        //Pour v�rifier si la position est valide, on va dans la liste des position (validGridPositionList)
        //et on check si on y trouve la gridPosition qui est pass�e en param�tre dans IsValidActionGridPosition(GridPosition gridPosition) via Contains()
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPosition();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }
}
