using System;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace Colony
{
    public abstract class BaseColonyAction : MonoBehaviour
    {
        [SerializeField] protected AnimationClip actionAnimationClip;
        
        protected ColonistMovement colonistMovement;
        protected GridPosition actionSpotGridPosition;
        
        protected bool isActive;
        protected Action OnActionComplete;
        
        protected AnimancerComponent animancerComponent;
        protected AnimancerState animancerState;

        protected virtual void Awake()
        {
            animancerComponent = GetComponent<AnimancerComponent>();
            colonistMovement = GetComponent<ColonistMovement>();
        }

        public abstract string GetActionName();
   
        public abstract void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition,
            Action onActionComplete, ColonyTask colonyTask);

        public abstract List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask);
        
        protected void ActionStart(Action onActionComplete)
        {
            isActive = true;
            OnActionComplete = onActionComplete;
        }

        protected void ActionComplete()
        {
            isActive = false;
            OnActionComplete();
        }
    }
}