using System;
using System.Collections.Generic;
using Colony;
using Random = UnityEngine.Random;

public class ColonyWanderAction : BaseColonyAction
{
    void Start()
    {
        colonistMovement = GetComponent<ColonistMovement>();
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
        colonistMovement.Move(callerGridPosition, wanderGridPosition, OnMovementCompleted);
    }

    private void OnMovementCompleted()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        return new List<GridPosition>();
    }
}
