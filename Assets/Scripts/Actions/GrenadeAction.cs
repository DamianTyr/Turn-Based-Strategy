using System;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    private GrenadeProjectile _grenadeProjectilePrefab;
    private int _maxThrowDistance = 7;
    
    public override string GetActionName()
    {
        return "Grenade";
    }

    public override void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition, Action onActionComplete)
    {
        GrenadeProjectile grenadeProjectile = Instantiate(_grenadeProjectilePrefab, Unit.GetWorldPosition(), Quaternion.identity);
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviorComplete);
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GetGridPosition();
        
        for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
        {
            for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!MissionGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                
                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > _maxThrowDistance) continue;
                
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
            ActionValue = 0
        };
    }

    private void OnGrenadeBehaviorComplete()
    {
        ActionComplete();
    }

    public void SetGrenadeProjectilePrefab(GrenadeProjectile grenadeProjectile)
    {
        _grenadeProjectilePrefab = grenadeProjectile;
    }
}
