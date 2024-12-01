using System;
using System.Collections.Generic;
using Colony;
using Random = UnityEngine.Random;

public class ColonyWanderAction : BaseColonyAction, IColonyAction
{
    private ColonyMoveAction _colonyMoveAction;
    private ColonyActionType _colonyActionType = ColonyActionType.Wandering;
    
    void Start()
    {
        _colonyMoveAction = GetComponent<ColonyMoveAction>();
    }
    
    public override string GetActionName()
    {
        return "Colony Wander Action";
    }

    public override void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition, Action onActionComplete, ColonyTask colonyTask)
    {
        List<GridPosition> validGridPositions = ColonyGrid.Instance.GetSquareAroundGridPosition(callerGridPosition, 2);
        List<GridPosition> reachableGridPositions = new List<GridPosition>();
        foreach (GridPosition validGridPosition in validGridPositions)
        {
            if (ColonyGrid.Instance.HasAnyOccupantOnGridPosition(validGridPosition)) continue;
            if (ColonyGrid.Instance.GetIsReservedAtGridPosition(validGridPosition)) continue;
            if (!Pathfinding.Instance.HasPath(callerGridPosition, validGridPosition)) continue;
            reachableGridPositions.Add(validGridPosition);
        }

        if (reachableGridPositions.Count == 0)
        {
            ActionStart(onActionComplete);
            ActionComplete();
            return;
        }

        int randomIndex = Random.Range(0, reachableGridPositions.Count);
        GridPosition wanderGridPosition = reachableGridPositions[randomIndex];
        ActionStart(onActionComplete);
        _colonyMoveAction.TakeAction(callerGridPosition, wanderGridPosition, OnMovementCompleted, colonyTask);
    }

    private void OnMovementCompleted()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        return new List<GridPosition>();
    }

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        return new AIAction
        {
            GridPosition = gridPosition,
            ActionValue = 5
        };
    }

    public ColonyActionType GetColonyActionType()
    {
        return _colonyActionType;
    }
}
