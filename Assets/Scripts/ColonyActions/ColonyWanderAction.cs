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

    public override void TakeAction(Action onActionComplete, ColonyTask colonyTask)
    {
        List<GridPosition> reachableGridPositions = GetValidActionGridPositionList(colonyTask);
        if (reachableGridPositions.Count == 0)
        {
            ActionStart(onActionComplete);
            ActionComplete();
            return;
        }

        int randomIndex = Random.Range(0, reachableGridPositions.Count);
        GridPosition wanderGridPosition = reachableGridPositions[randomIndex];
        ActionStart(onActionComplete);
        colonistMovement.Move(colonist.GetGridPosition(), wanderGridPosition, OnMovementCompleted);
    }

    private void OnMovementCompleted()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        GridPosition colonistGridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        List<GridPosition> validGridPositions = ColonyGrid.Instance.GetSquareAroundGridPosition(colonistGridPosition, 2);
        List<GridPosition> reachableGridPositions = new List<GridPosition>();
        foreach (GridPosition validGridPosition in validGridPositions)
        {
            if (ColonyGrid.Instance.HasAnyOccupantOnGridPosition(validGridPosition)) continue;
            if (ColonyGrid.Instance.GetIsReservedAtGridPosition(validGridPosition)) continue;
            if (!Pathfinding.Instance.HasPath(colonistGridPosition, validGridPosition)) continue;
            reachableGridPositions.Add(validGridPosition);
        }
        return reachableGridPositions;
    }

    public override ColonyActionType GetColonyActionType()
    {
        return ColonyActionType.Wandering;
    }
}
