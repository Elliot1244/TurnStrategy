using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGrid : MonoBehaviour
{
    public event EventHandler OnAnyUnitMovedGridPosition;

    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform _gridDebugObjectPrefab;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellSize;
    [SerializeField] private Transform _gridRoot;

    private GridSystem<GridObject> _gridSystem;
    private void Awake()
    {
        //S'il y a plus qu'une grille active à l'awake
        if (Instance != null)
        {
            Debug.LogError("More than one instance" + transform + " - " + Instance);
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;

        //Appel et Initailisation de la grille de 10 * 10
        _gridSystem = new GridSystem<GridObject>(_width, _height, _cellSize, (GridSystem<GridObject> g,GridPosition gridPosition) => new GridObject(g, gridPosition), _gridRoot);
        //_gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.SetUp(_width, _height, _cellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return _gridSystem.GetGridPosition(worldPosition);
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return _gridSystem.GetWorldPosition(gridPosition);
    }

    //Retourn une position valide sur la grille
    public bool IsValidgridPosition(GridPosition gridPosition)
    {
        return _gridSystem.IsValidgridPosition(gridPosition);
    }

    public int GetWidth()
    {
        return _gridSystem.GetWidth();
    }

    public int GetHeight()
    {
        return _gridSystem.GetHeight();
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        //On enlève l'ancien emplacement de l'unit
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        //On récupère le nouvel emplacement de l'unit et l'unit elle même
        AddUnitAtGridPosition(toGridPosition, unit);

        //On déclanche l'event dès qu'une unit bouge sur la grille
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    //Si la position de la grille est déjà occupée par une unit
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

}
