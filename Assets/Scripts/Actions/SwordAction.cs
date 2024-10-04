using System;
using System.Collections.Generic;
using UnityEngine;

 
public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;
    
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    
    private enum State
    {
        SwingingSwordBeforeHit, 
        SwingingSwordAfterHit
    }
    
    private int _maxSwordDistance = 1;
    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;
    
    public override string GetActionName()
    {
        return "Sword";
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _unitAnimator.EquipSword();
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
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        
        _state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = .7f;
        _stateTimer = beforeHitStateTime;
        
        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
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
}
