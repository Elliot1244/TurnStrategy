using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystemVisual : MonoBehaviour
{

    //Singleton
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform _gridSystemVisualPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> _gridVisualTypeMaterialList;
    [SerializeField] Transform _root;

    private GridSystemVisualSingle[,] _gridSystemVisualArray;

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType _gridVisualType;
        public Material _material;
    }


    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }

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
                    Instantiate(_gridSystemVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity, _root);

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

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for(int x = -range; x <= range; x++ )
        {
            for(int z = -range; z <= range; z++ )
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                //Si la position récupérée n'est pas valide sur la grille alors on continue l'itération de la boucle sans prendre en compte la valeur hors grille
                if (!LevelGrid.Instance.IsValidgridPosition(testGridPosition))
                {
                    continue;
                }

                //Si la distance de test est supérieure à la distance max de shoot possible, on ne prend pas en compte la case
                int _testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (_testDistance > range)
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }


    //Affiche les cases possibles en temps réel
    private void UpdateGrideVisual()
    {
        //On commence par cacher toutes les cases
        HideAllGridPosition();


        Unit selectdUnit = UnitActionSystem.Instance.GetSelectedUnit();
        //Récupère l'unit sélectionnée via UnitActionSystem et GetSelectedUnit
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();


        GridVisualType gridVisualType;
        //Change la couleur des cases selon l'action sélectionnée
        switch(selectedAction)
        {
            default: 
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectdUnit.GetGridPosition(), shootAction.GetMaxShootAction(), GridVisualType.RedSoft);
                break;
        }

        //On affiche les cases dispos pour l'unit sélectionnée
        ShowGridPositionList(selectedAction.GetValidActionGridPosition(), gridVisualType);
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

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach(GridVisualTypeMaterial gridVisualTypeMaterial in _gridVisualTypeMaterialList)
        {
            if(gridVisualTypeMaterial._gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial._material;
            }
        }
        return null;
    }
}
