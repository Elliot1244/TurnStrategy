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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(TryHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());


            //Si la souris est dans la limite des cases autorisées alors on bouge au click
            if(_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                _selectedUnit.GetMoveAction().Move(mouseGridPosition);
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            _selectedUnit.GetSpinAction().Spin();
        }
    }

    private bool TryHandleUnitSelection()
    {
        //On récupère la position de la souris dans le jeu
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Si la souris est sur un gameobject qui a le layer _unitLayerMask alors on sélectionne l'unit
        if (Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, _unitLayerMask))
            {
                if(rayCastHit.transform.TryGetComponent<Unit>(out Unit unit))
                    {
                        SetSelectedUnit(unit);
                        return true;
                     }
            }
            return false;
       
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }
}
