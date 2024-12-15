using System.Collections.Generic;
using Colony;
using UnityEngine;

public class Colonist : MonoBehaviour
{
    private GridPosition _gridPosition;
    [SerializeField] private ColonyWanderAction colonyWanderAction;
    [SerializeField] private bool isBusy;
    
    private ColonyTasksManager _colonyTasksManager;
    private ColonyTask _currentTask;
    
    private float _taskCheckTimer = 2f;

    private BaseColonyAction[] _baseColonyActionList; 
    
    private void Start()
    {
        _gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        _colonyTasksManager = FindObjectOfType<ColonyTasksManager>();
        _baseColonyActionList = GetComponents<BaseColonyAction>();
    }
    
    private void Update()
    {
        GridPosition newGridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            ColonyGrid.Instance.OccupantMovedGridPosition(transform, oldGridPosition, newGridPosition);
        }
        
        _taskCheckTimer -= Time.deltaTime;
        if (_taskCheckTimer <= 0)
        {
            _taskCheckTimer = 2f;
            if (isBusy) return;
            CheckForColonyTasks();
        }
    }

    private void CheckForColonyTasks()
    {
        List<ColonyTask> colonyTaskList = _colonyTasksManager.GetColonyTaskList();
        foreach (ColonyTask colonyTask in colonyTaskList)
        {
            foreach (BaseColonyAction baseColonyAction in _baseColonyActionList)
            {
                if (baseColonyAction.GetColonyActionType() != colonyTask.ActionType) continue;
                if (!baseColonyAction.CanPerformAction(colonyTask)) continue;
                if (colonyTask.AssignedColonist != null) continue;
                baseColonyAction.TakeAction(OnActionComplete, colonyTask);
                colonyTask.AssignedColonist = this;
                _currentTask = colonyTask;
                isBusy = true;
                return;
            }
        }
        
        if (!isBusy)
        {
            colonyWanderAction.TakeAction(OnActionComplete, null);
            isBusy = true;
        }
    }
    
    private void OnActionComplete()
    {
        _colonyTasksManager.RemoveTask(_currentTask);
        _currentTask = null;
        isBusy = false;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
}
