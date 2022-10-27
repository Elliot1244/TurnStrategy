using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] public Transform _actionButtonPrefab;
    [SerializeField] public Transform _actionButtonContainerPrefab;
    [SerializeField] public TextMeshProUGUI _actionPointsText;

    private List<ActionButtonUI> _actionButtonUIList;


    private void Awake()
    {
        _actionButtonUIList = new List<ActionButtonUI>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;

        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }
    private void CreateUnitActionButtons()
    {
        //On detruit les boutons
        foreach(Transform buttonTransform in _actionButtonContainerPrefab)
        {
            Destroy(buttonTransform.gameObject);
        }

        _actionButtonUIList.Clear();

        Unit _selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();


        //Pour chaque base action de type BaseAction dans l'unité sélectionnée qui récupère un tableau d'actions de base
        foreach(BaseAction baseAction in _selectedUnit.GetBaseActionArray())
        {
            //On instantie les buttons d'actions
            Transform actionButtonTransform =  Instantiate(_actionButtonPrefab, _actionButtonContainerPrefab);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            _actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();   
    }


    //Quand une action commence, on update le nombre de points d'action restant
    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }


    //Applique le visuel pour chaque button de l'UI qui se trouve dans la liste des bouttons
    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in _actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        _actionPointsText.text = "Action Points : " + selectedUnit.GetActionPoints();
    }


    //Lors de l'évènement de changement de tour, on update les points d'actions
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointChanged(object sender,EventArgs e)
    {
        UpdateActionPoints();
    }


}
