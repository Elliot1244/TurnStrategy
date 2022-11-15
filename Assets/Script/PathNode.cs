using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition _gridPosition;
    private int _gCost;
    private int _hCost;
    private int _fCost;

    private PathNode _cameFromPathNode;
    public PathNode(GridPosition gridPosition)
    {
        this._gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return _gridPosition.ToString();
    }

    public int GetGCost()
    {
        return _gCost;
    }

    public int GetHCost()
    {
        return _hCost;
    }

    public int GetFCost()
    {
        return _fCost;
    }

    public void SetGCost(int gCost)
    {
        this._gCost = gCost;
    }

    public void SetHCost(int hCost)
    {
        this._hCost = hCost;
    }

    public void CalculateFcost()
    {
        _fCost = _gCost + _hCost;
    }

    public void ResetCameFromPathNode()
    {
        _cameFromPathNode = null;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public void SetCameFromPathNode(PathNode pathNode)
    {
        _cameFromPathNode = pathNode;
    }

    public PathNode GetCameFromPathNode()
    {
      return  _cameFromPathNode;
    }
}
