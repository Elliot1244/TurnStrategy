using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> _gridSystem;
    private GridPosition _gridPosition;
    private DoorScript _door;

    //sans liste d'unit, probl�me si deux units se croisent sur la m�me case
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


    //Ajoute une unit � la liste d'unit
    public void AddUnit(Unit unit)
    {
        _unitList.Add(unit);
    }

    //Enl�ve une unit de la liste d'unit
    public void RemoveUnit(Unit unit)
    {
        _unitList.Remove(unit);
    }

    //R�cup�re la liste d'unit
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

    public DoorScript GetDoor()
    {
        return _door;
    }

    public void SetDoor(DoorScript door)
    {
        this._door = door;
    }
}
