using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPTrigger : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAllyShoot += ShootAction_OnAllyShoot;
    }

    private void ShootAction_OnAllyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        PP.Instance.PPTrigger();
    }
}
