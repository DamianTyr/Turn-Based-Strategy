using UnityEngine;

public class ColonyGridSystemVisual : BaseGridSystemVisual
{
    private static ColonyGridSystemVisual Instance { get; set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one Grid System Visual!" + transform + " - " + Instance);
            Destroy(gameObject);
        }

        Instance = this;
    }
    
    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        // Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        //
        // BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        //
        // GridSystemVisual.GridVisualType gridVisualType = GridSystemVisual.GridVisualType.White;
        // switch (selectedAction)
        // {
        //     case MoveAction:
        //         gridVisualType = GridSystemVisual.GridVisualType.White;
        //         break;
        //     case ShootAction shootAction:
        //         gridVisualType = GridSystemVisual.GridVisualType.Red;
        //         ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootingDistance(), GridSystemVisual.GridVisualType.RedSoft);
        //         break;
        //     case GrenadeAction:
        //         gridVisualType = GridSystemVisual.GridVisualType.Yellow;
        //         break;
        //     case MeleeAttackAction swordAction:
        //         gridVisualType = GridSystemVisual.GridVisualType.Red;
        //         ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridSystemVisual.GridVisualType.RedSoft);
        //         break;
        //     case InteractAction:
        //         gridVisualType = GridSystemVisual.GridVisualType.Blue;
        //         break;
        // }
        //
        // ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }
}
