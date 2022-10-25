using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActionArray;

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {

        //Au start on défini la position sur la grille de l'unit.
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    private void Update()
    {
        GridPosition _newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        //Si l'unit change de position sur la grille
        //(Opérateur non valide de base, voir Grid Position pour la création des opérateurs de la struct GridPosition)
        if(_newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, _newGridPosition);
            _gridPosition = _newGridPosition;
        }
    }

    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return _spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
       return _baseActionArray;
    }
}
