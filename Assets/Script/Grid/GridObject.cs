using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> _gridSystem;
    private GridPosition _gridPosition;
    private IInteractable _interactable;

    //sans liste d'unit, problème si deux units se croisent sur la même case
    private List<Unit> _unitList;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this._gridSystem = gridSystem;
        this._gridPosition = gridPosition;
        _unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach(Unit unit in _unitList)
        {
            unitString += unit + " \n";
        }
        return _gridPosition.ToString() + "\n" + unitString;
    }


    //Ajoute une unit à la liste d'unit
    public void AddUnit(Unit unit)
    {
        _unitList.Add(unit);
    }

    //Enlève une unit de la liste d'unit
    public void RemoveUnit(Unit unit)
    {
        _unitList.Remove(unit);
    }

    //Récupère la liste d'unit
    public List<Unit> GetUnitList()
    {
        return _unitList;
    }

    public bool HasAnyUnit()
    {
        return _unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if(HasAnyUnit())
        {
            return _unitList[0];
        }
        else
        {
            return null;
        }
    }

    public IInteractable GetInteractable()
    {
        return _interactable;
    }

    public void SetInteractable(IInteractable interactable)
    {
        this._interactable = interactable;
    }
}
