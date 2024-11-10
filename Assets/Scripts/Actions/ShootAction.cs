using System;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum State
    {
        Aiming, 
        Shooting, 
        Cooloff
    }
    
    private int _maxShootDistance = 7;
    private Transform _shootPointTransform;
    private EquipableGun _equipableGun;
    
    private State _state;
    private float _stateTimer;
    private Combat.Unit _targetUnit;
    
    private AnimancerState _animancerStatePreShot;
    private bool _canShootBullets;
    
    private void OnEnable()
    {
        Debug.Log("Shoot Action Added");
    }

    void Update()
    {
        if (!IsActive) return;

        _stateTimer -= Time.deltaTime;
        
        switch (_state)
        {
            case State.Aiming:
                float rotateSpeed = 10f;
                Vector3 aimDirection = (_targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (_canShootBullets)
                {
                    Shoot();
                    _canShootBullets = false;
                }

                break;
            case State.Cooloff:
                break;
        }
        if (_stateTimer <= 0)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        _targetUnit.Damage(_equipableGun.GetDamage(), transform);
        
        BulletProjectile bulletProjectile = Instantiate(_equipableGun.GetBulletProjectile(), _shootPointTransform.position,Quaternion.identity);
        Vector3 targetUnitShootAtPosition = _targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = _shootPointTransform.position.y;
        bulletProjectile.Setup(targetUnitShootAtPosition);
        
        _animancerStatePreShot = AnimancerComponent.States.Current;
        AnimancerState animancerState = AnimancerComponent.Play(_equipableGun.GetAttackAnimationClip(), _equipableGun.GetAttackAnimationFadeTime());
        animancerState.Events(this).OnEnd += OnShootAnimationEnd;
        
        ScreenShake.Instance.Shake();
    }

    private void OnShootAnimationEnd()
    {
        AnimancerComponent.Play(_animancerStatePreShot, _equipableGun.GetAttackAnimationFadeTime());
    }

    private void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                float shootingStateTime = .1f;
                _stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                _state = State.Cooloff;
                float cooloffStateTime = .5f;
                _stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = Unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    private List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > _maxShootDistance) continue;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Combat.Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == Unit.IsEnemy()) continue;

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight, shootDirection, Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), _equipableGun.GetObstaclesLayerMask()))
                {
                    //Blocked by obstacle
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
    
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        _state = State.Aiming;
        
        float aimingStateTime = 1f;
        _stateTimer = aimingStateTime;
        _canShootBullets = true;
        
        ActionStart(onActionComplete);
    }

    public Combat.Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    public int GetMaxShootingDistance()
    {
        return _maxShootDistance;
    }
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Combat.Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 100 + Mathf.RoundToInt((1- targetUnit.GetHealthNormalized()) * 100f)
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }


    public void SetShootPointTransform(Transform shootPointTransform)
    {
        _shootPointTransform = shootPointTransform;
    }

    public void SetEquipableGun(EquipableGun equipableGun)
    {
        _equipableGun = equipableGun;
    }
}
