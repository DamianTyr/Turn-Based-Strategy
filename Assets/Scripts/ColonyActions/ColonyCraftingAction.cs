using System;
using System.Collections.Generic;
using Colony;

public class ColonyCraftingAction : BaseColonyAction
{
    public override string GetActionName()
    {
        return "Craft Action";
    }

    public override void TakeAction(GridPosition callerGridPosition, GridPosition actionSpot, Action onActionComplete,
        ColonyTask colonyTask)
    {
        //_currentMineable = ColonyGrid.Instance.GetMineableAtGridPosition(colonyTask.GridPosition);
        actionSpotGridPosition = actionSpot;
        ColonyGrid.Instance.ReserveActionSpot(actionSpotGridPosition);
        
        colonyMoveAction.TakeAction(callerGridPosition, actionSpotGridPosition, OnMovementComplete, colonyTask);
        ActionStart(onActionComplete);
    }

    private void OnMovementComplete()
    {
        throw new NotImplementedException();
    }

    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        throw new NotImplementedException();
    }
    
    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        throw new NotImplementedException();
    }
}
