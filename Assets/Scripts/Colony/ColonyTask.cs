using System;

[Serializable]
public class ColonyTask
{
    public GridPosition GridPosition { get; set; }
    public ColonyActionType ActionType { get; set; }

    public Colonist AssignedColonist { get; set; }

    public ColonyTask(GridPosition gridPosition, ColonyActionType actionType)
    {
        GridPosition = gridPosition;
        ActionType = actionType;
    }
}
