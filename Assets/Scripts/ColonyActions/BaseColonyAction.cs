using System;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace Colony
{
    public abstract class BaseColonyAction : MonoBehaviour
    {
        [SerializeField] protected AnimationClip actionAnimationClip;

        protected Colonist colonist;
        protected ColonistMovement colonistMovement;
        protected GridPosition actionSpotGridPosition;
        
        protected bool isActive;
        protected Action OnActionComplete;
        
        protected AnimancerComponent animancerComponent;
        protected AnimancerState animancerState;

        protected IColonyActionTarget colonyActionTarget;
        protected bool isPerformingAction;
        protected float actionAnimationDuration = 1.5f;
        protected float timer;
        
        protected virtual void Awake()
        {
            colonist = GetComponent<Colonist>();
            colonistMovement = GetComponent<ColonistMovement>();
            animancerComponent = GetComponent<AnimancerComponent>();
        }

        public abstract string GetActionName();
   
        public abstract void TakeAction(Action onActionComplete, ColonyTask colonyTask);

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

        public bool CanPerformAction(ColonyTask colonyTask)
        {
            return GetValidActionGridPositionList(colonyTask).Count > 0;
        }

        public abstract ColonyActionType GetColonyActionType();
    }
}