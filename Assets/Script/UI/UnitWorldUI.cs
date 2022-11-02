using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _actionPointsText;
    [SerializeField] private Unit _unit;
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private HealthSystem _healthSystem;


    private void Start()
    {
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
        _healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        _actionPointsText.text = _unit.GetActionPoints().ToString();
    }


    //Quand l'évènement de changement du nombre de point d'action intervient, alors on appelle la méthode pour mettre à jour le nombre de point d'action
    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateHealthBar()
    {
        _healthBarImage.fillAmount = _healthSystem.GetHealthNormalized();
    }


    //On met à jour la barre de vie à chaque fois que des dommages sont appliqués
    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
