using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform _ragdollPrefab;
    [SerializeField] private Transform _originalRootBone;

    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.OnDead += HealthSystem_OnDead;
    }


    //M�thode o� lors de l'�v�nement OnDead, on fait spawner la ragdoll � la place de l'unit
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.SetUp(_originalRootBone);
    }
}
