using System;
using ColonyActions;
using Grid;

namespace Colony
{
    [Serializable]
    public class ColonyTask
    {
        public GridPosition GridPosition { get; set; }
        public ColonyActionType ActionType { get; set; }

        public IColonyActionTarget colonyActionTarget { get; set; }

        public Colonist AssignedColonist { get; set; }

        public ColonyTask(GridPosition gridPosition, ColonyActionType actionType, IColonyActionTarget colonyActionTarget)
        {
            GridPosition = gridPosition;
            ActionType = actionType;
            this.colonyActionTarget = colonyActionTarget;
        }
    }
}
