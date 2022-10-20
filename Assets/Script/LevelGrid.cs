using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform _gridDebugObjectPrefab;

    private GridSystem _gridSystem;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance" + transform + " - " + Instance);
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;

        //Appel et Initailisation de la grille de 10 * 10
        _gridSystem = new GridSystem(10, 10, 2f);
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetUnit(unit);
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public void ClearUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetUnit(null);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return _gridSystem.GetGridPosition(worldPosition);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        //On enlève l'ancien emplacement de l'unit
        ClearUnitAtGridPosition(fromGridPosition);

        //On récupère le nouvel emplacement de l'unit et l'unit elle même
        SetUnitAtGridPosition(toGridPosition, unit);
    }

}
