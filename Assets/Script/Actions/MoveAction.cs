using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private int _maxMoveDistance;

    private Vector3 _targetPosition;

    protected override void Awake()
    {

        base.Awake();
        //A l'initialisation, le point d'arriv�e est la position de l'unit
        _targetPosition = transform.position;
    }

    private void Update()
    {
        //Direction de d�placement de l'unit
        Vector3 _moveDirection = (_targetPosition - transform.position).normalized;

        if(!_isActive)
        {
            return;
        }

        if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            //activation de l'animation
            _animator.SetBool("isWalking", true);

            //D�placement en lui m�me
            transform.position += _moveDirection * _moveSpeed * Time.deltaTime; 
        }
        else
        {

            //Une fois l'unit arriv�e � destination
            _animator.SetBool("isWalking", false);
            _isActive = false;
            _onActionComplete();
        }

        //Rotation de l'unit
        transform.forward = Vector3.Lerp(transform.forward, _moveDirection, Time.deltaTime * _rotateSpeed);
    }


    //Fait bouger l'unit vers la position de la souris
    public void Move(GridPosition gridPosition, Action _onActionComplete)
    {
        this._onActionComplete = _onActionComplete;
        this._targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        _isActive = true;
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPosition();
        //Pour v�rifier si la position est valide, on va dans la liste des position (validGridPositionList)
        //et on check si on y trouve la gridPosition qui est pass�e en param�tre dans IsValidActionGridPosition(GridPosition gridPosition) via Contains()
        return validGridPositionList.Contains(gridPosition);
    }


    //R�cup�re une liste de gridposition sur lesquelles on peut bouger
    public List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPosition = new List<GridPosition>();


        //On r�cup�re la position sur la grille de l'unit
        GridPosition unitGridPosition = _unit.GetGridPosition();

        //On cherche depuis la gauche vers la droite sur l'axe x
        for(int x = -_maxMoveDistance; x <= _maxMoveDistance; x++ )
        {
            for(int z = -_maxMoveDistance; z <= _maxMoveDistance; z++ )
            {
                GridPosition offSetGridPosition =  new GridPosition(x, z);

                //Op�rateur + / - non existant pour une struct, voir GridPosition Ligne 52 � 60
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;


                //Si la position r�cup�r�e n'est pas valide sur la grille alors on continue l'it�ration de la boucle sans prendre en compte la valeur hors grille
                if(!LevelGrid.Instance.IsValidgridPosition(testGridPosition))
                {
                    continue;
                }


                //Si la position sur la grille est la m�me que celle o� se trouve d�j� l'unit, on ne prend pas en compte cette position
                if(unitGridPosition == testGridPosition)
                {
                    continue;
                }

                //Si la position sur la grille est occup�e par une autre unit, on ne prend pas en compte cette position
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }


                validGridPosition.Add(testGridPosition);
            }
        }

        return validGridPosition;
    }

    public override string GetActionName()
    {
        return "Move";
    }

}
