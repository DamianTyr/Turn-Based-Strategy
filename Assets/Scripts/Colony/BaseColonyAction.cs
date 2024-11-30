using System;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace Colony
{
    public abstract class BaseColonyAction : MonoBehaviour
    {
        public static event EventHandler OnAnyActionStarted;
        public static event EventHandler OnAnyActionCompleted;
        
        protected bool IsActive;
        protected Action OnActionComplete;

        protected AnimancerComponent AnimancerComponent;

        protected virtual void Awake()
        {
            AnimancerComponent = GetComponent<AnimancerComponent>();
        }

        public abstract string GetActionName();
   
        public abstract void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition,
            Action onActionComplete, ColonyTask colonyTask);

        public abstract List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask);

        public virtual int GetCost()
        {
            return 1;
        }

        protected void ActionStart(Action onActionComplete)
        {
            IsActive = true;
            OnActionComplete = onActionComplete;

            OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void ActionComplete()
        {
            IsActive = false;
            OnActionComplete();

            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        public Transform GetHolderTransform()
        {
            return transform;
        }
        
        public abstract AIAction GetAIAction(GridPosition gridPosition);
    }
}