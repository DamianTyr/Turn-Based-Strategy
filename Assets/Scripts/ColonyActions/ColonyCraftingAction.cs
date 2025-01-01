using System.Collections.Generic;
using Colony;
using UnityEngine;

public class ColonyCraftingAction : BaseColonyAction
{
    private void Update()
    {
        if (!isPerformingAction) return;
        timer += Time.deltaTime;
        if (timer > actionAnimationDuration)
        {
            timer = 0;
            currentColonyActionTarget.ProgressTask(10, OnTaskCopleted);
        }
    }
    
    public override string GetActionName()
    {
        return "Craft Action";
    }
    
    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition colonistGridPosition = colonist.GetGridPosition();
        
        GridPosition craftingGridPos = colonyTask.GridPosition;
        if (!ColonyGrid.Instance.IsValidGridPosition(craftingGridPos)) return validGridPositionList;
        if (colonistGridPosition == craftingGridPos) return validGridPositionList;
        if (ColonyGrid.Instance.HasAnyOccupantOnGridPosition(craftingGridPos)) return validGridPositionList;
        if (ColonyGrid.Instance.GetIsReservedAtGridPosition(craftingGridPos)) return validGridPositionList;
        if (!Pathfinding.Instance.IsWalkableGridPosition(craftingGridPos)) return validGridPositionList;
        if (!Pathfinding.Instance.HasPath(colonistGridPosition,craftingGridPos)) return validGridPositionList;
        
        validGridPositionList.Add(craftingGridPos);
        return validGridPositionList;
    }

    public override ColonyActionType GetColonyActionType()
    {
        return ColonyActionType.Crafting;
    }
}
