using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{

    //Singleton
    public static UnitActionSystem Instance { get; private set; }

    [SerializeReference] private Unit _selectedUnit;
    [SerializeReference] private LayerMask _unitLayerMask;

    private bool _isBusy;
    private BaseAction _selectedAction;

    //Evenement changement d'unit sélectionnée
    public event EventHandler OnSelectedUnitChanged;

    private void Awake()
    {
        //Vérifie qu'il n'y ait qu'une instance sinon destruction
        if(Instance != null)
        {
            Debug.LogError("More than one instance" + transform + " - " + Instance);
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(_selectedUnit);
    }

    private void Update()
    {
        //Si l'unit est busy (est en train de faire une action), on ne fait rien
        if(_isBusy)
        {
            return;
        }


        if (TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if(Input.GetMouseButton(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            switch (_selectedAction)
            {
                case MoveAction moveAction:
                    //Si la souris est dans la limite des cases autorisées alors on bouge au click
                    if (_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
                    {
                        SetBusy();
                        moveAction.Move(mouseGridPosition, ClearBusy);
                    }
                    break;
                case SpinAction spinAction:
                    SetBusy();
                    spinAction.Spin(ClearBusy);
                    break;
            }
        }
    }


    //L'unit est busy
    private void SetBusy()
    {
        _isBusy = true;
    }

    //L'unit n'est pas/plus busy
    private void ClearBusy()
    {
        _isBusy = false;
    }

    private bool TryHandleUnitSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //On récupère la position de la souris dans le jeu
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Si la souris est sur un gameobject qui a le layer _unitLayerMask alors on sélectionne l'unit
            if (Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, _unitLayerMask))
            {
                if (rayCastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }



    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;
    }


    //Retourne l'unit sélectionnée actuellement
    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }
}
