using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    //Constante qui d�finie le nombre de point d'action par unit�
    private const int ACTION_POINT_MAX = 2;

    [SerializeField] private bool _isEnemy;

    public static event EventHandler OnAnyActionPointChanged;

    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private HealthSystem _healthSystem;
    private BaseAction[] _baseActionArray;
    private int _actionPoints = ACTION_POINT_MAX;


    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {

        //Au start on d�fini la position sur la grille de l'unit.
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        _healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void Update()
    {
        GridPosition _newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        //Si l'unit change de position sur la grille
        //(Op�rateur non valide de base, voir Grid Position pour la cr�ation des op�rateurs de la struct GridPosition)
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

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }


    //Si on peut d�penser des actions points alors on le fait
    public bool TrySpendActionPoints(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(_actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;

        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }


    //R�cup�re le nombre de points d'action
    public int GetActionPoints()
    {
        return _actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {

        if(IsEnemy() && !TurnSystem.Instance.IsPlayerTurn() || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
            {
            _actionPoints = ACTION_POINT_MAX;

            OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return _isEnemy;
    }


    //M�thode qui prend en param�tre un montant de damage puis appelle la m�thode Damage du script HealthSystem pour r�duire la vie
    public void Damage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);
    }


    //M�thode inscrite � l'�v�nement OnDead, qui d�truit le gamobject sans point de vie restant 
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {

        //Supprime la coloration de la case indiquant qu'il y a quelqu'un dessus
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);

        Destroy(gameObject);
    }
}
