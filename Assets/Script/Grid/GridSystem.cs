using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystem<TGridObject>
{

    private int _width;
    private int _height;
    private float _cellSize;
    Transform _root;


    //La virgule indique un tableau à deux dimensions
    private TGridObject[,] _gridObjectArray;
    public GridSystem(int width, int height, float cellSize, 
        Func<GridSystem<TGridObject>, GridPosition, TGridObject> cretaeGridObject,
        Transform root=null)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        _root = root;

        _gridObjectArray = new TGridObject[width, height];


        //Construit la grille selon les valeurs données en Ligne 8 à 10
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                //new GridObject(this, gridPosition);
                _gridObjectArray[x, z] = cretaeGridObject(this, gridPosition);
            }
        }
    }

    //Converti le vector3 en int
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize)
            );
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity, _root);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }

    //Test si la position sur la grille est valide
    public bool IsValidgridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
               gridPosition.z >= 0 && 
               gridPosition.x < _width && 
               gridPosition.z < _height;
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }


}