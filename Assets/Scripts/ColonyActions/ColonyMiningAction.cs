using System;
using System.Collections.Generic;
using Colony;
using UnityEngine;

public class ColonyMiningAction : BaseColonyAction
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
        return "Mining Action";
    }
    
    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition colonistGridPosition = colonist.GetGridPosition();
        
        List<GridPosition> testMiningSpots = ColonyGrid.Instance.GetSquareAroundGridPosition(colonyTask.GridPosition, 1);
        foreach (GridPosition testGridPosition in testMiningSpots)
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
        return ColonyActionType.Mining;
    }
}
