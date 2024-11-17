using System;
using Mission;
using UnityEngine;

public class MissionGridSystemVisual : BaseGridSystemVisual
{
    private static MissionGridSystemVisual Instance { get; set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one Grid System Visual!" + transform + " - " + Instance);
            Destroy(gameObject);
        }

        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        
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

    protected void LevelGrid_OnOnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }
    
    protected override void UpdateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType = GridVisualType.White;
        switch (selectedAction)
        {
            case MoveAction:
                gridVisualType = GridVisualType.White;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootingDistance(), GridVisualType.RedSoft);
                break;
            case GrenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case MeleeAttackAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
            case InteractAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }
        
        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    protected Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
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
