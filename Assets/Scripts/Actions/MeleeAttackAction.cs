using System;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class MeleeAttackAction : BaseAction
{
    private enum State
    {
        SwingingSwordBeforeHit, 
        SwingingSwordAfterHit
    }
    
    private int _maxSwordDistance = 1;
    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;

    private AnimationClip _meleeAtackAniationClip;
    private AnimancerState _animancerStatePreAttack;
    
    public override string GetActionName()
    {
        return "Melee Attack";
    }

    private void Update()
    {
        if (!IsActive) return;
        
        _stateTimer -= Time.deltaTime;
        
        switch (_state)
        {
            case State.SwingingSwordBeforeHit:
                float rotateSpeed = 10f;
                Vector3 aimDirection = (_targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }
        if (_stateTimer <= 0)
        {
            NextState();
        }
    }
    
    private void NextState()
    {
        switch (_state)
        {
            case State.SwingingSwordBeforeHit:
                _state = State.SwingingSwordAfterHit;
                float afterHitStateTime = .5f;
                _stateTimer = afterHitStateTime;
                _targetUnit.Damage(200, transform);
                ScreenShake.Instance.Shake(1f);
                break;
            case State.SwingingSwordAfterHit:
                //OnMeleeAttackActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        
        _state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = .4f;
        _stateTimer = beforeHitStateTime;

        _animancerStatePreAttack = AnimancerComponent.States.Current;
        AnimancerState animancerState = AnimancerComponent.Play(_meleeAtackAniationClip);
        animancerState.Events(this).OnEnd += OnMeleeAttackAnimationEnd;
        
        ActionStart(onActionComplete);
    }

    private void OnMeleeAttackAnimationEnd()
    {
        AnimancerComponent.Play(_animancerStatePreAttack);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GetGridPosition();
        
        for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++)
        {
            for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == Unit.IsEnemy()) continue;
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 200
        };
    }

    public int GetMaxSwordDistance()
    {
        return _maxSwordDistance;
    }
    
    public void SetMeleeAttackAnimationClip(AnimationClip animationClip)
    {
        _meleeAtackAniationClip = animationClip;
    }
}
