using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] public Transform _actionButtonPrefab;
    [SerializeField] public Transform _actionButtonContainerPrefab;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        CreateUnitActionButtons();
    }
    private void CreateUnitActionButtons()
    {
        //On detruit les boutons
        foreach(Transform buttonTransform in _actionButtonContainerPrefab)
        {
            Destroy(buttonTransform.gameObject);
        }

        Unit _selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();


        //Pour chaque base action de type BaseAction dans l'unité sélectionnée qui récupère un tableau d'actions de base
        foreach(BaseAction baseAction in _selectedUnit.GetBaseActionArray())
        {
            //On instantie les buttons d'actions
            Transform actionButtonTransform =  Instantiate(_actionButtonPrefab, _actionButtonContainerPrefab);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }
}
