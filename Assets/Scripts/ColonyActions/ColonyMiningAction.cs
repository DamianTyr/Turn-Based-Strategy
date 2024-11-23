using System;
using System.Collections.Generic;
using Animancer;
using Colony;
using UnityEngine;

public class ColonyMiningAction : BaseColonyAction
{
    private ColonyActionType _colonyActionType = ColonyActionType.Mining;
    private ColonyMoveAction _colonyMoveAction;
    [SerializeField] private AnimationClip miningAnimationClip;
    private AnimancerComponent animancerComponent;
    private AnimancerState _animancerState;
    private GridPosition mineableGridPosition;

    private bool isMining;
    private float mineSwingDuration = 1.5f;
    private float mineTimer = 0;

    private Mineable _currentMineable;
    
    
    private void Start()
    {
        _colonyMoveAction = GetComponent<ColonyMoveAction>();
        animancerComponent = GetComponent<AnimancerComponent>();
    }

    private void Update()
    {
        if (!isMining) return;
        mineTimer += Time.deltaTime;
        if (mineTimer > mineSwingDuration)
        {
            mineTimer = 0;
            _currentMineable.Mine(10, OnMiningCompleted);
        }
    }

    public override string GetActionName()
    {
        return "Mining Action";
    }
    
    public override void TakeAction(GridPosition callerGridPosition, GridPosition miningSpot, Action onActionComplete, ColonyTask colonyTask)
    {
        _currentMineable = ColonyGrid.Instance.GetMineableAtGridPosition(colonyTask.GridPosition);
        _colonyMoveAction.TakeAction(callerGridPosition, miningSpot, OnMovementComplete);
        ActionStart(onActionComplete);
    }

    private void OnMovementComplete()
    {
        isMining = true;
        transform.LookAt(_currentMineable.transform);
        _animancerState = animancerComponent.States.Current;
        animancerComponent.Play(miningAnimationClip);
    }

    private void OnMiningCompleted()
    {
        isMining = false;
        _currentMineable = null;
        animancerComponent.Play(_animancerState, .4f);
        OnActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition colonistGridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        
        List<GridPosition> testMiningSpots = ColonyGrid.Instance.GetSqaureAroundGridPosition(colonyTask.GridPosition, 1);
        foreach (GridPosition testGridPosition in testMiningSpots)
        {
            if (!ColonyGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
            if (colonistGridPosition == testGridPosition) continue;
            if (ColonyGrid.Instance.HasAnyOccupantOnGridPosition(testGridPosition))continue;
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

    public ColonyActionType GetColonyActionType()
    {
        return _colonyActionType;
    }
    
   
}
