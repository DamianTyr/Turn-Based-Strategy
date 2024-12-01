using System;
using System.Collections.Generic;
using Colony;
using UnityEngine;

public class ColonyMoveAction : BaseColonyAction
{
    [SerializeField] private AnimationClip _idleAnimationClip;
    [SerializeField] private AnimationClip _runAnimationClip;
    
    private List<Vector3> _positionList;
    private int _currentPositionIndex;
    
    private ColonyGrid _baseGrid;
    
    private void Start()
    {
        animancerComponent.Play(_idleAnimationClip);
        _baseGrid = FindObjectOfType<ColonyGrid>();
    }

    void Update()
    {
        if (!isActive) return;

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
                animancerComponent.Play(_idleAnimationClip, .3f);
                ActionComplete();
            }
        }
    }
    
    public override void TakeAction(GridPosition callerGridPosition, GridPosition gridPosition, Action onActionComplete, ColonyTask colonyTask)
    {
        List<GridPosition> pathGridPostionList = Pathfinding.Instance.FindPath(callerGridPosition, gridPosition, out int pathLenght);
        if (pathGridPostionList == null)
        {
            Debug.Log("Aborted Action, Path count 0");
            ActionComplete();
            return;
        }

        _currentPositionIndex = 0;
        _positionList = new List<Vector3>();
        
        foreach (GridPosition pathGridPosition in pathGridPostionList)
        {
            _positionList.Add(_baseGrid.GetWorldPosition(pathGridPosition));
        }
        
        animancerComponent.Play(_runAnimationClip, 0.3f);
        ActionStart(onActionComplete);
    }
    
    public override string GetActionName()
    {
        return "ColonyMoveAction";
    }
    
    public override List<GridPosition> GetValidActionGridPositionList(ColonyTask colonyTask)
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        return gridPositions;
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
