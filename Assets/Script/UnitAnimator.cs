using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _spawner;
    [SerializeField] private Transform _swordTransform;


    private void Awake()
    {
       if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += shootAction_OnShoot;

        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;

        }
    }

    private void Start()
    {
        NoSword();
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        DrawSword();
        _animator.SetTrigger("stab");
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        NoSword();
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        _animator.SetBool("isWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        _animator.SetBool("isWalking", false);
    }

    private void shootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        _animator.SetTrigger("shoot");

        Transform bulletProjectileTransform = Instantiate(_bulletProjectilePrefab, _spawner.position, Quaternion.identity);
        BulletProjectile bulletProjectile =  bulletProjectileTransform.GetComponent<BulletProjectile>();
        
        //La balle reste à la même hauteur que celle du _spawner.
        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = _spawner.position.y;
        bulletProjectile.SetUp(targetUnitShootAtPosition);
    }

    private void DrawSword()
    {
        _swordTransform.gameObject.SetActive(true);
    }

    private void NoSword()
    {
        _swordTransform.gameObject.SetActive(false);
    }
}

