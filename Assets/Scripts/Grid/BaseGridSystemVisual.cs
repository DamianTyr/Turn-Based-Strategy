using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseGridSystemVisual : MonoBehaviour
{
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    
    public enum GridVisualType
    {
        White,
        Blue, 
        Red, 
        RedSoft,
        Yellow 
    }
    
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] protected List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    
    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;
    
    protected virtual void Start()
    {
        BaseGrid baseGrid = GameObject.FindObjectOfType<BaseGrid>();
        
        int width = baseGrid.GetWidth();
        int height = baseGrid.GetHeight();

        _gridSystemVisualSingleArray = new GridSystemVisualSingle[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab,baseGrid.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingleTransform.parent = transform;
                _gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
        
        UpdateGridVisual();
    }
    
    protected void HideAllGridPosition()
    {
        for (int x = 0; x < _gridSystemVisualSingleArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridSystemVisualSingleArray.GetLength(1); z++)
            {
                _gridSystemVisualSingleArray[x,z].Hide();
            }
        }
    }

    protected void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <=+ range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!MissionGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                
                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > range) continue;
                
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, GridVisualType.RedSoft);
    }

    protected void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <=+ range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!MissionGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, GridVisualType.RedSoft);
    }

    protected void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.X, gridPosition.Z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    protected virtual void UpdateGridVisual()
    {
        HideAllGridPosition();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.LogError("Could not find appropriate grid visual type material");
        return null;
    }
}
