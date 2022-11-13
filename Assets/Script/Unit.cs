using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    //Constante qui définie le nombre de point d'action par unité
    private const int ACTION_POINT_MAX = 2;

    [SerializeField] private bool _isEnemy;

    public static event EventHandler OnAnyActionPointChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private GridPosition _gridPosition;
    private HealthSystem _healthSystem;
    private BaseAction[] _baseActionArray;
    private int _actionPoints = ACTION_POINT_MAX;


    private void Awake()
    {
        _baseActionArray = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {

        //Au start on défini la position sur la grille de l'unit.
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        _healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition _newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        //Si l'unit change de position sur la grille
        //(Opérateur non valide de base, voir Grid Position pour la création des opérateurs de la struct GridPosition)
        if(_newGridPosition != _gridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = _newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, _newGridPosition);
        }
    }

    //Fonction générique qui récupère forçément une BaseAction
    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in _baseActionArray)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }


    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
       return _baseActionArray;
    }


    //Récupère la position de l'unit
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }


    //Si on peut dépenser des actions points alors on le fait
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


    //Récupère le nombre de points d'action
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


    //Méthode qui prend en paramètre un montant de damage puis appelle la méthode Damage du script HealthSystem pour réduire la vie
    public void Damage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);
    }


    //Méthode inscrite à l'évènement OnDead, qui détruit le gamobject sans point de vie restant 
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        //Supprime la coloration de la case indiquant qu'il y a quelqu'un dessus
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);

        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return _healthSystem.GetHealthNormalized();
    }
}
