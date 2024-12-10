using System;
using System.Collections.Generic;
using Colony;
using UnityEngine;

public class ColonyMiningAction : BaseColonyAction
{
    private IColonyActionTarget _colonyActionTarget;
    private bool _isPerformingAction;
    private float _actionAnimationDuration = 1.5f;
    private float _timer;
    
    private void Update()
    {
        if (!_isPerformingAction) return;
        _timer += Time.deltaTime;
        if (_timer > _actionAnimationDuration)
        {
            _timer = 0;
            _colonyActionTarget.ProgressTask(10, OnMiningCompleted);
        }
    }

    public override string GetActionName()
    {
        return "Mining Action";
    }
    
    public override void TakeAction(GridPosition callerGridPosition, GridPosition miningSpot, Action onActionComplete, ColonyTask colonyTask)
    {
        _colonyActionTarget = colonyTask.colonyActionTarget;
        actionSpotGridPosition = miningSpot;
        ColonyGrid.Instance.ReserveActionSpot(actionSpotGridPosition);
        
        colonyMoveAction.TakeAction(callerGridPosition, actionSpotGridPosition, OnMovementComplete, colonyTask);
        ActionStart(onActionComplete);
    }

    private void OnMovementComplete()
    {
        _isPerformingAction = true;
        
        //TODO: FIX THIS!
        //transform.LookAt(_colonyActionTarget);
        animancerState = animancerComponent.States.Current;
        animancerComponent.Play(actionAnimationClip);
    }

    private void OnMiningCompleted()
    {
        _isPerformingAction = false;
        _colonyActionTarget = null;
        animancerComponent.Play(animancerState, .4f);
        ColonyGrid.Instance.RemoveReserveActionSpot(actionSpotGridPosition);
        OnActionComplete();
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

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        return new AIAction
        {
            GridPosition = gridPosition,
            ActionValue = 10
        };
    }
}
