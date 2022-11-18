using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction
{
    public GridPosition _gridPosition;

    public int _actionValue;

    public override string ToString() => $"{_gridPosition}";
}
