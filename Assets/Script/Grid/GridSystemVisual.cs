using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystemVisual : MonoBehaviour
{

    //Singleton
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform _gridSystemVisualPrefab;

    private GridSystemVisualSingle[,] _gridSystemVisualArray;

    private void Awake()
    {

        if (Instance != null)
        {
            Debug.LogError("More than one instance" + transform + " - " + Instance);
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {

        //On met les valeurs de width et height dans un tableau à deux dimensions
        _gridSystemVisualArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        //Instantie le prefab visuel sur toute la grille
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform gridSystemVisualSingleTransform = 
                    Instantiate(_gridSystemVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                _gridSystemVisualArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        //On s'abonne à l'event du changement d'action
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;

        //On s'abonne à l'event du mouvement d'une unit
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        UpdateGrideVisual();
    }

  

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                _gridSystemVisualArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualArray[gridPosition.x, gridPosition.z].Show();
        }
    }


    //Affiche les cases possibles en temps réel
    private void UpdateGrideVisual()
    {
        //On commence par cacher toutes les cases
        HideAllGridPosition();

        //Récupère l'unit sélectionnée via UnitActionSystem et GetSelectedUnit
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        //On affiche les cases dispos pour l'unit sélectionnée
        ShowGridPositionList(
          selectedAction.GetValidActionGridPosition()
        );
    }


    //Quand l'action de l'unit change
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGrideVisual();
    }

    //Quand une unit bouge
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGrideVisual();
    }
}
