using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public  abstract class BaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected bool _isActive;

    //Fontion delegete pour que spin() puisse la recevoir comme paramètre afin qu'elle s'execute puis execute automatiquement la fonction mise en paramètre
    //Le type Action (avec using System) remplace "delegate void".
    protected Action _onActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

}
