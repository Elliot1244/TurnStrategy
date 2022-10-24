using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void Update()
    {
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
        Unit _selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        //On affiche les cases dispos pour l'unit sélectionnée
        ShowGridPositionList(
          _selectedUnit.GetMoveAction().GetValidActionGridPosition()
        );
    }
}
