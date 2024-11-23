using System;
using System.Collections.Generic;
using UnityEngine;

public class ColonyMoveAction : BaseAction
{
 
    private ColonyActionType _colonyActionType = ColonyActionType.Mining;
    
    private List<Vector3> _positionList;
    [SerializeField] private int maxMoveDistance;
    
    private int _currentPositionIndex;
    [SerializeField] private AnimationClip _idleAnimationClip;
    [SerializeField] private AnimationClip _runAnimationClip;
    private ColonyGrid _baseGrid;
    
    private void Start()
    {
        AnimancerComponent.Play(_idleAnimationClip);
        _baseGrid = FindObjectOfType<ColonyGrid>();
    }

    void Update()
    {
        if (!IsActive) return;

        Vector3 targetPosition = _positionList[_currentPositionIndex];
        Vector3 moveDirection = (targetPosition - ((Component)this).transform.position).normalized;
        
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(((Component)this).transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        
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
            _positionList.Add(_baseGrid.GetWorldPosition(pathGridPosition));
        }
        
        AnimancerComponent.Play(_runAnimationClip, 0.3f);
        ActionStart(onActionComplete);
    }
    
    public override string GetActionName()
    {
        return "ColonyMoveAction";
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition colonistGridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = colonistGridPosition + offsetGridPosition;

                if (!ColonyGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (colonistGridPosition == testGridPosition) continue;
                //if (ColonyGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) continue;
                if (!Pathfinding.Instance.HasPath(colonistGridPosition,testGridPosition)) continue;
                int pathfindingDistanceMultiplier = 10;
                if (Pathfinding.Instance.GetPathLenght(colonistGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier) continue;
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        return new AIAction()
        {
            GridPosition = gridPosition,
            ActionValue = 0
        };
    }
}
