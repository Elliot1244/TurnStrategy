using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamTrigger : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExplode += GrenadeProjectile_OnAnyGrenadeExplode;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
    }

    private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
    {
        ShakeCam.Instance.Shake(5f);
    }

    private void GrenadeProjectile_OnAnyGrenadeExplode(object sender, EventArgs e)
    {
        ShakeCam.Instance.Shake(15f);
        Debug.Log("test grenade event");
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ShakeCam.Instance.Shake();
    }

}
