using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamTrigger : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ShakeCam.Instance.Shake();
    }

}
