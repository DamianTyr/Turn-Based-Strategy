using System;
using System.Collections.Generic;
using Colony;

public class ColonyCraftingAction : BaseColonyAction
{
    public override string GetActionName()
    {
        return "Craft Action";
    }

    public override void TakeAction(Action onActionComplete, ColonyTask colonyTask)
    {
        colonyActionTarget = colonyTask.colonyActionTarget;
        actionSpotGridPosition = GetValidActionGridPositionList(colonyTask)[0];;;
        ColonyGrid.Instance.ReserveActionSpot(actionSpotGridPosition);
        
        colonistMovement.Move(colonist.GetGridPosition(), actionSpotGridPosition, OnMovementComplete);
        ActionStart(onActionComplete);
    }

    private void OnMovementComplete()
    {
        isPerformingAction = true;
        transform.LookAt(colonyActionTarget.transformPosition);
        animancerState = animancerComponent.States.Current;
        animancerComponent.Play(actionAnimationClip);
    }

    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition colonistGridPosition = colonist.GetGridPosition();
        
        List<GridPosition> testCraftingSpots = ColonyGrid.Instance.GetSquareAroundGridPosition(colonyTask.GridPosition, 1);
        foreach (GridPosition testGridPosition in testCraftingSpots)
        {
            if (!ColonyGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
            if (colonistGridPosition == testGridPosition) continue;
            if (ColonyGrid.Instance.HasAnyOccupantOnGridPosition(testGridPosition))continue;
            if (ColonyGrid.Instance.GetIsReservedAtGridPosition(testGridPosition)) continue;
            if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) continue;
            if (!Pathfinding.Instance.HasPath(colonistGridPosition,testGridPosition)) continue;
            validGridPositionList.Add(testGridPosition);
        }
        return validGridPositionList;
    }

    public override ColonyActionType GetColonyActionType()
    {
        return ColonyActionType.Crafting;
    }
}
