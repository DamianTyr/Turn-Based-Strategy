using System;
using UnityEngine;

namespace Grid
{
    public class GridSystem<TGridObject> 
    {
        private int _width;
        private int _height;
        private float _cellSize;
        private TGridObject[,] _gridObjectArray;
    
        public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _gridObjectArray = new TGridObject[width, height];
        
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    _gridObjectArray[x,z] = createGridObject(this, gridPosition);
                }
            }
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return new Vector3(gridPosition.X, 0, gridPosition.Z) * _cellSize;
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
                    Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                    GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    gridDebugObject.SetGridObject(GetGridObject(gridPosition));
                }
            }
        }

        public TGridObject GetGridObject(GridPosition gridPosition)
        {
            return _gridObjectArray[gridPosition.X, gridPosition.Z];
        }

        public bool IsValidGridPosition(GridPosition gridPosition)
        {
            return gridPosition.X >= 0 && gridPosition.Z >= 0 && gridPosition.X < _width && gridPosition.Z < _height;
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public void Save()
        {
            Debug.Log("Test Save triggered");
            ES3.Save("Test Array Save", _gridObjectArray);
        }
    }
}
