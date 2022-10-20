using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Removing Monobehavior to be able to use a constructor
public class GridSystem
{
    private int _height;
    private int _width;
    private float _cellSize;

    //The coma is here because we want a 2 dimaension array
    private GridObject[,] _gridObjectArray;


    public GridSystem(int _height, int _width, float _cellSize)
    {
        this._height = _height;
        this._width = _width;
        this._cellSize = _cellSize;

        //Store the values of the grid
        _gridObjectArray = new GridObject[_width, _height];

        for(int x = 0; x < _width; x++)
        {
            for(int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                _gridObjectArray[x, z] = new GridObject(this, gridPosition);
            }
        }
       
    }


    //Convert grid position into a vector3
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }


    //Convert the vector3 into a grid position
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            //RoundToInt to convert a float into a int
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize)
            );
    }

    public void CreateDebugObject(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

               Transform debugTransform =  GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
               GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
               gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }

}
