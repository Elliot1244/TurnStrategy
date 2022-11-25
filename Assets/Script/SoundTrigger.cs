using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundTrigger : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        SoundScript.Instance.SoundTrigger();
    }

    /*private void BaseAction_OnAnyActionEnded(object sender, EventArgs e)
    {
        //Si l'action finie est le shoot alors
        if (sender is ShootAction)
        {
          SoundScript.Instance.SoundOff();
        }
    }*/
 }
