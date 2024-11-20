using System;
using System.Collections.Generic;
using Colony;

public class ColonyWanderAction : BaseAction, IColonyAction
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

    public override void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition, Action onActionComplete)
    {
        GridPosition wanderPosition = ColonyGrid.Instance.GetRandomGridPositionInSquare(callerGridPosition);
        _colonyMoveAction.TakeAction(callerGridPosition, wanderPosition, OnMovementCompleted);
        ActionStart(onActionComplete);
    }

    private void OnMovementCompleted()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
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
