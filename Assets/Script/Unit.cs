using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private Animator _animator;
    private Vector3 _targetPosition;
    private GridPosition _gridPosition;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Start()
    {

        //Au start on défini la position sur la grille de l'unit.
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(_gridPosition, this);
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            _animator.SetBool("isWalking", true);
            Vector3 _moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += _moveDirection * _moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, _moveDirection, Time.deltaTime * _rotateSpeed);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }

        GridPosition _newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        //Si l'unit change de position sur la grill
        //(Opérateur non valide de base, voir Grid Position pour la création des opérateurs de la struct GridPosition)
        if(_newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, _newGridPosition);
            _gridPosition = _newGridPosition;
        }
    }

    public void Move(Vector3 _targetPosition)
    {
        this._targetPosition = _targetPosition;
    }

   
}
