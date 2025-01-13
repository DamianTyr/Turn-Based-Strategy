using System;
using System.Collections.Generic;
using Animancer;
using Enemy;
using Grid;
using UnityEngine;
using Mission;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;
    
    protected Unit unit;
    protected bool IsActive;
    protected Action OnActionComplete;
    
    protected AnimancerComponent AnimancerComponent;
    
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        AnimancerComponent = GetComponent<AnimancerComponent>();
    }
    
    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition, Action onActionComplete);
    
    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        IsActive = true;
        OnActionComplete = onActionComplete;
        
        OnAnyActionStarted?.Invoke(this,EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        IsActive = false;
        OnActionComplete();
        
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public UnityEngine.Transform GetHolderTransform()
    {
        return ((Component)this).transform;
    }

    public AIAction GetBestAIAction()
    {
        List<AIAction> enemyAIActionList = new List<AIAction>();
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            AIAction aiAction = GetAIAction(gridPosition);
            enemyAIActionList.Add(aiAction);
        }
        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((AIAction a, AIAction b) => b.ActionValue - a.ActionValue);
            return enemyAIActionList[0];
        }
        else
        {
            return null;
        }
    }

    public abstract AIAction GetAIAction(GridPosition gridPosition);
    
}
