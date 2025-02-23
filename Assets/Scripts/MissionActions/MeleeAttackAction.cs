using System;
using System.Collections.Generic;
using Animancer;
using Enemy;
using Grid;
using UnityEngine;
using Mission;

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
    
    private AnimancerState _animancerStatePreAttack;
    private EquipableWeapon _equipableWeapon;
    
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
                float rotateSpeed = 15f;
                Vector3 aimDirection = (_targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
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
                float afterHitStateTime = 1f;
                _stateTimer = afterHitStateTime;
                _targetUnit.TakeDamage(_equipableWeapon.GetDamage(), transform);
                ScreenShake.Instance.Shake(1f);
                break;
            case State.SwingingSwordAfterHit:
                ActionComplete();
                break;
        }
    }

    public override void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = MissionGrid.Instance.GetOccupantAtGridPosition(gridPosition).GetComponent<Unit>();
        
        
        _state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = .4f;
        _stateTimer = beforeHitStateTime;

        _animancerStatePreAttack = AnimancerComponent.States.Current;
        AnimancerState animancerState = AnimancerComponent.Play(_equipableWeapon.GetAttackAnimationClip(), _equipableWeapon.GetAttackAnimationFadeTime());
        animancerState.Events(this).OnEnd += OnMeleeAttackAnimationEnd;
        
        ActionStart(onActionComplete);
    }

    private void OnMeleeAttackAnimationEnd()
    {
        AnimancerComponent.Play(_animancerStatePreAttack);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }
    
    private List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        
        for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++)
        {
            for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!MissionGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (!MissionGrid.Instance.HasAnyOccupantOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = MissionGrid.Instance.GetOccupantAtGridPosition(testGridPosition).GetComponent<Unit>();
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        return new AIAction
        {
            GridPosition = gridPosition,
            ActionValue = 200
        };
    }

    public int GetMaxSwordDistance()
    {
        return _maxSwordDistance;
    }
    
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public void SetEquipableMeleeWeapon(EquipableWeapon equipableWeapon)
    {
        _equipableWeapon = equipableWeapon;
    }
}
