using System;
using Grid;

namespace Mission
{
    public interface IInteractable
    { 
        void Interact(Action OnInteractionComplete);

        void AddToGridPositionList(GridPosition gridPosition);
    }
}
