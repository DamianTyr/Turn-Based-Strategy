using System;
using System.Collections.Generic;
using Animancer;
using Colony;
using UnityEngine;

public class ColonyMiningAction : BaseAction, IColonyAction
{
    private ColonyActionType _colonyActionType = ColonyActionType.Mining;
    private ColonyMoveAction _colonyMoveAction;
    [SerializeField] private AnimationClip miningAnimationClip;
    private AnimancerComponent animancerComponent;
    private AnimancerState _animancerState;
    private GridPosition mineableGridPosition;

    private bool isMining;
    private float mineSwingDuration = 1.5f;
    private float mineTimer = 0;

    private Mineable _currentMineable;
    
    private void Start()
    {
        _colonyMoveAction = GetComponent<ColonyMoveAction>();
        animancerComponent = GetComponent<AnimancerComponent>();
    }

    private void Update()
    {
        if (!isMining) return;
        mineTimer += Time.deltaTime;
        if (mineTimer > mineSwingDuration)
        {
            mineTimer = 0;
            _currentMineable.Mine(10, OnMiningCompleted);
        }
    }

    public override string GetActionName()
    {
        return "Mining Action";
    }

    public override void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition, Action onActionComplete)
    {
        mineableGridPosition = gridPosition;
        _currentMineable = ColonyGrid.Instance.GetMineableAtGridPosition(mineableGridPosition);
        GridPosition miningSpot =  ColonyGrid.Instance.GetSqaureAroundGridPosition(gridPosition, 1)[0];
        _colonyMoveAction.TakeAction(callerGridPosition, miningSpot, OnMovementComplete);
        ActionStart(onActionComplete);
    }

    private void OnMovementComplete()
    {
        isMining = true;
        transform.LookAt(_currentMineable.transform);
        _animancerState = animancerComponent.States.Current;
        animancerComponent.Play(miningAnimationClip);
    }

    private void OnMiningCompleted()
    {
        isMining = false;
        _currentMineable = null;
        animancerComponent.Play(_animancerState, .4f);
        OnActionComplete();
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
            ActionValue = 10
        };
    }

    public ColonyActionType GetColonyActionType()
    {
        return _colonyActionType;
    }
    
   
}
