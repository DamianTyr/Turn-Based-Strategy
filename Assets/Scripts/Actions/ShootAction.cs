using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum State
    {
        Aiming, 
        Shooting, 
        Cooloff
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    
    public event EventHandler<OnShootEventArgs> OnShoot;
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private int _maxShootDistance = 7;
    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;
    private bool canShootBullets;
    
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
                if (canShootBullets)
                {
                    Shoot();
                    canShootBullets = false;
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
        _targetUnit.Damage(40, transform);
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = _targetUnit,
            shootingUnit = Unit
        });
        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = _targetUnit,
            shootingUnit = Unit
        });
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
            default:
                throw new ArgumentOutOfRangeException();
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
    

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
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

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == Unit.IsEnemy()) continue;

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHight = 1.7f;
                if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHight, shootDirection, Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), obstaclesLayerMask))
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
        canShootBullets = true;
        
        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    public int GetMaxShootingDistance()
    {
        return _maxShootDistance;
    }
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
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
}
