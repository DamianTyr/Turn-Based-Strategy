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
        
        protected bool isActive;
        protected Action OnActionComplete;
        
        protected AnimancerComponent animancerComponent;
        protected AnimancerState animancerState;

        protected float actionAnimationDuration = 1.5f;
        protected float timer;
        
        protected IColonyActionTarget currentColonyActionTarget;
        protected GridPosition actionSpotGridPosition;
        protected bool isPerformingAction;
        
        protected virtual void Awake()
        {
            colonist = GetComponent<Colonist>();
            colonistMovement = GetComponent<ColonistMovement>();
            animancerComponent = GetComponent<AnimancerComponent>();
        }

        public abstract string GetActionName();

        public virtual void TakeAction(Action onActionComplete, ColonyTask colonyTask)
        {
            currentColonyActionTarget = colonyTask.colonyActionTarget;
            actionSpotGridPosition = GetValidActionGridPositionList(colonyTask)[0];;;
            ColonyGrid.Instance.ReserveActionSpot(actionSpotGridPosition);
        
            colonistMovement.Move(colonist.GetGridPosition(), actionSpotGridPosition, OnMovementComplete);
            ActionStart(onActionComplete);
        }
        
        protected virtual void OnMovementComplete()
        {
            isPerformingAction = true;
            transform.LookAt(currentColonyActionTarget.transformPosition);
            animancerState = animancerComponent.States.Current;
            animancerComponent.Play(actionAnimationClip);
        }

        protected virtual void OnTaskCopleted()
        {
            isPerformingAction = false;
            currentColonyActionTarget = null;
            animancerComponent.Play(animancerState, .4f);
            ColonyGrid.Instance.RemoveReserveActionSpot(actionSpotGridPosition);
            ActionComplete();
        }

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