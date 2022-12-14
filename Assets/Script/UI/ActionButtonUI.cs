using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _selectedGameObject;

    private BaseAction _baseAction;


    public void SetBaseAction(BaseAction baseAction)
    {

        this._baseAction = baseAction;

        //On modifie le text de chaque bouttons selon les actions des bouttons
        _textMeshPro.text = baseAction.GetActionName().ToUpper();

        _button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {

        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        _selectedGameObject.SetActive(selectedBaseAction == _baseAction);    
    }
}
