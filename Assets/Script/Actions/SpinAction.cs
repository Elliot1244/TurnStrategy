using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{

    [SerializeField] float _totalSpinAmount;

    //Fontion delegete pour que spin() puisse la recevoir comme paramètre afin qu'elle s'execute puis execute automatiquement la fonction mise en paramètre
    //Le type Action (avec using System) remplace "delegate void".
    //private Action _onSpinComplete;


    private void Update()
    {
        if(!_isActive)
        {
            return;
        }

        //Rotation de 360°
        float _spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, _spinAmount, 0);

        _totalSpinAmount += _spinAmount;


        //Si le total de la rotation == 360, on arrête la rotation
        if(_totalSpinAmount >= 360f)
        {

            _isActive = false;

            //Appel du delegate une fois la rotation finie
            _onActionComplete();
        }
    }

    //Fonction active un spin de l'unit et reset le _totalSptinAmount
    public void Spin(Action _onActionComplete)
    {
        this._onActionComplete = _onActionComplete;
        _isActive = true;
        _totalSpinAmount = 0f;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
