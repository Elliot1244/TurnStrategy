using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem _gridSystem;
    private GridPosition _gridPosition;
    private Unit _unit;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this._gridSystem = gridSystem;
        this._gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return _gridPosition.ToString() + "\n" + _unit;
    }

    public void SetUnit(Unit unit)
    {
        this._unit = unit;
    }

    public Unit GetUnit()
    {
        return _unit;
    }
}
