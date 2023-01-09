using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{

    //Singleton
    public static UnitActionSystem Instance { get; private set; }

    [SerializeReference] private Unit _selectedUnit;
    [SerializeReference] private LayerMask _unitLayerMask;

    private bool _isBusy;
    private BaseAction _selectedAction;

    //Evenement changement d'unit s�lectionn�e
    public event EventHandler OnSelectedUnitChanged;

    //Evenement changement d'action s�lectionn�e
    public event EventHandler OnSelectedActionChanged;

    //Evenement Busy
    public event EventHandler<bool> OnBusyChanged;

    //Evenement lors de l'activation d'une action
    public event EventHandler OnActionStarted;



    private void Awake()
    {
        //V�rifie qu'il n'y ait qu'une instance sinon destruction
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

        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        //Si la souris est sur un �l�ment de l'UI
        if(EventSystem.current.IsPointerOverGameObject())
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
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());


            //Si l'action choisie est sur une position valide de la grille
            if(!_selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }
            //L'unit essaye de d�penser les points d'action
            if (!_selectedUnit.TrySpendActionPoints(_selectedAction))
            {
                return;
            }
            SetBusy();
            _selectedAction.TakeAction(mouseGridPosition, ClearBusy);


            //Trigger l'event OnActionStarted
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }


    //L'unit est busy
    private void SetBusy()
    {
        _isBusy = true;

        OnBusyChanged?.Invoke(this, _isBusy);
    }

    //L'unit n'est pas/plus busy
    private void ClearBusy()
    {
        _isBusy = false;

        OnBusyChanged?.Invoke(this, _isBusy);
    }


    //Fonction qui s�lectionne l'unit
    private bool TryHandleUnitSelection()
    {
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            //On r�cup�re la position de la souris dans le jeu
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            //Si la souris est sur un gameobject qui a le layer _unitLayerMask alors on s�lectionne l'unit
            if (Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, _unitLayerMask))
            {
                if (rayCastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {

                    //Si le cleck se fait sur une unit� d�j� s�lectionn�e
                    if(unit == _selectedUnit)
                    {
                        return false;
                    }

                    if(unit.IsEnemy())
                    {
                        return false;
                    }

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
        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }



    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
}


    //Retourne l'unit s�lectionn�e actuellement
    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }
}
