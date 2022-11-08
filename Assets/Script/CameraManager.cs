using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _actionCamera;

    private void Start()
    {
        //On écoute l'event qui indique qu'une action a commencée
        BaseAction.OnAnyActionStart += BaseAction_OnAnyActionStarted;


        //On écoute l'event qui indique que l'action est finie
        BaseAction.OnAnyActionEnd += BaseAction_OnAnyActionEnded;

        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        _actionCamera.SetActive(true);
    }

    private void HideActionCamera()
    {
        _actionCamera.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {

        switch (sender)
        {
            //Si l'action commencée est le shoot alors
            case ShootAction shootAction:
                Unit shootingUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();


                //Met la camera à hauteur d'épaule
                Vector3 CameraHeight = Vector3.up * 1.7f;

                //Calcule la distance entre la cible et l'unit qui tire
                Vector3 ShootDirection = (targetUnit.GetWorldPosition() - shootingUnit.GetWorldPosition()).normalized;

                Vector3 ShoulderOffset = Quaternion.Euler(0, 90, 0) * ShootDirection * 0.5f;

               Vector3 ActionCameraPosition = shootingUnit.GetWorldPosition() + CameraHeight + ShoulderOffset + (ShootDirection * -1);


                //La caméra se positionne à hauteur d'épaule puis regarde dans la direction de l'unit ciblée
                _actionCamera.transform.position = ActionCameraPosition;
                _actionCamera.transform.LookAt(targetUnit.GetWorldPosition() + CameraHeight);
                ShowActionCamera();

                break;
        }           
    }

    private void BaseAction_OnAnyActionEnded(object sender, EventArgs e)
    {
        //Si l'action finie est le shoot alors
        if (sender is ShootAction)
        {
            HideActionCamera();
        }
    }
}
