using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    

    private List<Unit> _unitList;
    public List<Unit> _friendlyUnitnitList;
    public List<Unit> _enemyUnitList;

    private void Awake()
    {
        //V�rifie qu'il n'y ait qu'une instance sinon destruction
        if (Instance != null)
        {
            Debug.LogError("More than one UnitManager" + transform + " - " + Instance);
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;

        _unitList = new List<Unit>();
        _friendlyUnitnitList = new List<Unit>();
        _enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }


    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e )
    {
        Unit unit = sender as Unit;

        _unitList.Add(unit);

        if(unit.IsEnemy())
        {
            _enemyUnitList.Add(unit);
        }
        else
        {
            _friendlyUnitnitList.Add(unit);
        }
    }


    //Quand une unit� meurt
    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        //On l'enl�ve de la liste des unit�s
        _unitList.Remove(unit);

        //Si l'unit �tait un vilain, on l'enl�ve de la liste des m�chant sinon on l'enl�ve de la liste des units du joueur
        if (unit.IsEnemy())
        {
            _enemyUnitList.Remove(unit);
        }
        else
        {
            _friendlyUnitnitList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList()
    {
        return _unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return _friendlyUnitnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return _enemyUnitList;
    }
}
