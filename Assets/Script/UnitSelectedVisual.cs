using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnityActionSystem_OnSelectedUnitChanged;
        UpdateVisual();
    }


    private void UnityActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == _unit)
        {
            _meshRenderer.enabled = true;
        }
        else
        {
            _meshRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        //Dès qu'une unit est détruite, on se désabonne de l'évènement de changement de ladite unit
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnityActionSystem_OnSelectedUnitChanged;
    }
}
