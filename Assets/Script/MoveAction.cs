using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private int _maxMoveDistance;

    private Vector3 _targetPosition;
    private Unit _unit;

    private void Awake()
    {
        //A l'initialisation, le point d'arrivée est la position de l'unit
        _targetPosition = transform.position;
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            //activation de l'animation
            _animator.SetBool("isWalking", true);

            //Direction de déplacement de l'unit
            Vector3 _moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += _moveDirection * _moveSpeed * Time.deltaTime;

            //Rotation de l'unit
            transform.forward = Vector3.Lerp(transform.forward, _moveDirection, Time.deltaTime * _rotateSpeed);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }

    public void Move(GridPosition gridPosition)
    {
        this._targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPosition();
        //Pour vérifier si la position est valide, on va dans la liste des position (validGridPositionList)
        //et on check si on y trouve la gridPosition qui est passée en paramètre dans IsValidActionGridPosition(GridPosition gridPosition) via Contains()
        return validGridPositionList.Contains(gridPosition);
    }


    //Récupère une liste de gridposition sur lesquelles on peut bouger
    public List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPosition = new List<GridPosition>();


        //On récupère la position sur la grille de l'unit
        GridPosition unitGridPosition = _unit.GetGridPosition();

        //On cherche depuis la gauche vers la droite sur l'axe x
        for(int x = -_maxMoveDistance; x <= _maxMoveDistance; x++ )
        {
            for(int z = -_maxMoveDistance; z <= _maxMoveDistance; z++ )
            {
                GridPosition offSetGridPosition =  new GridPosition(x, z);

                //Opérateur + / - non existant pour une struct, voir GridPosition Ligne 52 à 60
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;


                //Si la position récupérée n'est pas valide sur la grille alors on continue l'itération de la boucle sans prendre en compte la valeur hors grille
                if(!LevelGrid.Instance.IsValidgridPosition(testGridPosition))
                {
                    continue;
                }


                //Si la position sur la grille est la même que celle où se trouve déjà l'unit, on ne prend pas en compte cette position
                if(unitGridPosition == testGridPosition)
                {
                    continue;
                }

                //Si la position sur la grille est occupée par une autre unit, on ne prend pas en compte cette position
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }


                validGridPosition.Add(testGridPosition);
            }
        }

        return validGridPosition;
    }

}
