using System;
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
            colonyActionTarget.ProgressTask(10, OnTaskCopleted);
        }
    }
    
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

    private void OnTaskCopleted()
    {
        isPerformingAction = false;
        colonyActionTarget = null;
        animancerComponent.Play(animancerState, .4f);
        ColonyGrid.Instance.RemoveReserveActionSpot(actionSpotGridPosition);
        ActionComplete();
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
