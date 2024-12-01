using System;
using System.Collections.Generic;
using Colony;
using UnityEngine;

public class ColonyBuildingAction : BaseColonyAction
{
    private PlacedFurnitureGhost _placedFurnitureGhost;
    private float _actionAnimationDuration = 1.5f;
    private bool _isPerformingAction;
    private float _timer;
    
    void Update()
    {
        if (!_isPerformingAction) return;
        _timer += Time.deltaTime;
        if (_timer > _actionAnimationDuration)
        {
            _timer = 0;
            _placedFurnitureGhost.ProgressTask(10, OnBuildingCompleted);
        }
    }

    public override string GetActionName()
    {
        return "Building Action";
    }
    

    public override void TakeAction(GridPosition callerGridPosition, GridPosition buildingSpot, Action onActionComplete, ColonyTask colonyTask)
    {
        _placedFurnitureGhost = ColonyGrid.Instance.GetFurnitureGhostAtGridPosition(colonyTask.GridPosition);
        actionSpotGridPosition = buildingSpot;
        ColonyGrid.Instance.ReserveActionSpot(actionSpotGridPosition);
        
        colonyMoveAction.TakeAction(callerGridPosition, actionSpotGridPosition, OnMovementComplete, colonyTask);
        ActionStart(onActionComplete);
    }
    
    private void OnMovementComplete()
    {
        _isPerformingAction = true;
        transform.LookAt(_placedFurnitureGhost.transform);
        animancerState = animancerComponent.States.Current;
        animancerComponent.Play(actionAnimationClip);
    }

    private void OnBuildingCompleted()
    {
        _isPerformingAction = false;
        _placedFurnitureGhost = null;
        animancerComponent.Play(animancerState, .4f);
        ColonyGrid.Instance.RemoveReserveActionSpot(actionSpotGridPosition);
        ActionComplete();
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
