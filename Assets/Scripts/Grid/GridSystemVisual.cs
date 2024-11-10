using System;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    private static GridSystemVisual Instance { get; set; }

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
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    
    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one Grid System Visual!" + transform + " - " + Instance);
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        int width = LevelGrid.Instance.GetWidht();
        int height = LevelGrid.Instance.GetHeight();

        _gridSystemVisualSingleArray = new GridSystemVisualSingle[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab,LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingleTransform.parent = transform;
                _gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
        
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged; 
        //LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnOnAnyUnitMovedGridPosition;

        BaseAction.OnAnyActionStarted += BaseAction_OnOnAnyStartMoving;
        BaseAction.OnAnyActionCompleted += BaseAction_OnOnAnyStopMoving;
        
        UpdateGridVisual();
    }

    private void BaseAction_OnOnAnyStopMoving(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void BaseAction_OnOnAnyStartMoving(object sender, EventArgs e)
    {
        HideAllGridPosition();
    }

    private void LevelGrid_OnOnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void HideAllGridPosition()
    {
        for (int x = 0; x < _gridSystemVisualSingleArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridSystemVisualSingleArray.GetLength(1); z++)
            {
                _gridSystemVisualSingleArray[x,z].Hide();
            }
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <=+ range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                
                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > range) continue;
                
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, GridVisualType.RedSoft);
    }
    
    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <=+ range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, GridVisualType.RedSoft);
    }

    private void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.X, gridPosition.Z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType = GridVisualType.White;
        switch (selectedAction)
        {
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootingDistance(), GridVisualType.RedSoft);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case MeleeAttackAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }
        
        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
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
