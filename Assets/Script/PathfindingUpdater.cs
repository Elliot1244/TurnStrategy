using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        Destructible.OnAnyDestructed += Destructible_OnAnyDestructed;
    }

    private void Destructible_OnAnyDestructed(object sender, EventArgs e)
    {
        Destructible destructible = sender as Destructible;
        Pathfinding.Instance.SetisWalkableGridPosition(destructible.GetGridPosition(), true);
    }
}
