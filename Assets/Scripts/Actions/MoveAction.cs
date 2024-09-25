using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private List<Vector3> _positionList;
    [SerializeField] private int maxMoveDistance;
    private int _currentPositionIndex;
    
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    
    void Update()
    {
        if (!IsActive) return;

        Vector3 targetPosition = _positionList[_currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }
        else
        {
            _currentPositionIndex++;
            if (_currentPositionIndex >= _positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }
    
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPostionList = Pathfinding.Instance.FindPath(Unit.GetGridPosition(), gridPosition, out int pathLenght);
        
        _currentPositionIndex = 0;
        _positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPostionList)
        {
            _positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
   
        OnStartMoving?.Invoke(this, EventArgs.Empty);  
        ActionStart(onActionComplete);
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    { 
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = Unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (unitGridPosition == testGridPosition) continue;
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) continue;
                if (!Pathfinding.Instance.HasPath(unitGridPosition,testGridPosition)) continue;
                int pathfindingDistanceMultiplier = 10;
                if (Pathfinding.Instance.GetPathLenght(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier) continue;
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = Unit.GetActiom<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = targetCountAtGridPosition * 10
        };
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
