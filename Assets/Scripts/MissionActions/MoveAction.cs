using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Grid;
using Mission;
using UnityEngine;

public class MoveAction : BaseAction
{
    private List<Vector3> _positionList;
    [SerializeField] private int maxMoveDistance;
    
    private int _currentPositionIndex;
    private AnimationClip _idleAnimationClip;
    private AnimationClip _runAnimationClip;
    private MissionGrid _missionGrid;
    
    private void Start()
    {
        _missionGrid = FindObjectOfType<MissionGrid>();
    }
    
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
                AnimancerComponent.Play(_idleAnimationClip, .3f);
                ActionComplete();
            }
        }
    }
    
    public override void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPostionList = Pathfinding.Instance.FindPath(callerGridPosition, gridPosition, out int pathLenght);
        
        _currentPositionIndex = 0;
        _positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPostionList)
        {
            _positionList.Add(_missionGrid.GetWorldPosition(pathGridPosition));
        }
        
        AnimancerComponent.Play(_runAnimationClip, 0.3f);
        ActionStart(onActionComplete);
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    { 
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!MissionGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (unitGridPosition == testGridPosition) continue;
                if (MissionGrid.Instance.HasAnyOccupantOnGridPosition(testGridPosition)) continue;
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) continue;
                if (!Pathfinding.Instance.HasPath(unitGridPosition,testGridPosition)) continue;
                int pathfindingDistanceMultiplier = 10;
                if (Pathfinding.Instance.GetPathLenght(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier) continue;
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        unit.TryGetComponent(out ShootAction shootAction);
        if (shootAction)
        {
            int targetCountAtGridPosition = shootAction.GetTargetCountAtPosition(gridPosition);
            return new AIAction
            {
                GridPosition = gridPosition,
                ActionValue = targetCountAtGridPosition * 10
            };
        }
        unit.TryGetComponent(out MeleeAttackAction meleeAttackAction);
        if (meleeAttackAction)
        {
            int targetCountAtGridPosition = meleeAttackAction.GetTargetCountAtPosition(gridPosition);
            return new AIAction
            {
                GridPosition = gridPosition,
                ActionValue = targetCountAtGridPosition * 30
            };
        }

        return new AIAction()
        {
            GridPosition = gridPosition,
            ActionValue = 0
        };
    }

    public override string GetActionName()
    {
        return "Move";
    }
    
    public void SetAnimationClips(AnimationClip idleAnimationClip, AnimationClip runAnimationClip)
    {
        _idleAnimationClip = idleAnimationClip;
        _runAnimationClip = runAnimationClip;

        AnimancerComponent.Play(_idleAnimationClip);
    }
}
