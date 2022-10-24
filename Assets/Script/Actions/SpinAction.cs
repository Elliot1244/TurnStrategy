using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{

    [SerializeField] float _totalSpinAmount;


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
        }
    }

    //Fonction active un spin de l'unit et reset le _totalSptinAmount
    public void Spin()
    {
        _isActive = true;
        _totalSpinAmount = 0f;
    }
}
