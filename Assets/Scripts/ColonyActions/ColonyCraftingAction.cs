using System;
using System.Collections.Generic;
using Colony;
using UnityEngine;

public class ColonyCraftingAction : BaseColonyAction
{
    private IColonyActionTarget _colonyActionTarget;
    private bool _isPerformingAction;
    private float _actionAnimationDuration = 1.5f;
    private float _timer;
    
    public override string GetActionName()
    {
        return "Craft Action";
    }

    public override void TakeAction(GridPosition callerGridPosition, GridPosition actionSpot, Action onActionComplete,
        ColonyTask colonyTask)
    {
        _colonyActionTarget = colonyTask.colonyActionTarget;
        actionSpotGridPosition = actionSpot;
        ColonyGrid.Instance.ReserveActionSpot(actionSpotGridPosition);
        
        colonistMovement.Move(callerGridPosition, actionSpotGridPosition, OnMovementComplete);
        ActionStart(onActionComplete);
    }

    private void OnMovementComplete()
    {
        _isPerformingAction = true;
        transform.LookAt(_colonyActionTarget.transformPosition);
        animancerState = animancerComponent.States.Current;
        animancerComponent.Play(actionAnimationClip);
    }

    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition colonistGridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        
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
}
