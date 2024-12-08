using System;
using System.Collections.Generic;
using Colony;
using UnityEngine;

public class Colonist : MonoBehaviour
{
    private GridPosition _gridPosition;
    [SerializeField] private ColonyWanderAction colonyWanderAction;
    [SerializeField] private ColonyMiningAction colonyMiningAction;
    [SerializeField] private ColonyBuildingAction colonyBuildingAction;
    [SerializeField] private bool isBusy;
    
    private ColonyTasksManager _colonyTasksManager;
    private ColonyTask currentTask;

    private float taskCheckTimer = 2f;
    
    private void Start()
    {
        _gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        _colonyTasksManager = FindObjectOfType<ColonyTasksManager>();
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
        
        taskCheckTimer -= Time.deltaTime;
        if (taskCheckTimer <= 0)
        {
            taskCheckTimer = 2f;
            if (isBusy) return;
            CheckForColonyTasks();
        }
    }

    private void CheckForColonyTasks()
    {
        List<ColonyTask> colonyTaskList = _colonyTasksManager.GetColonyTaskList();
        foreach (ColonyTask colonyTask in colonyTaskList)
        {
            if (colonyTask.AssignedColonist != null) continue;
            
            switch (colonyTask.ActionType)
            {
                case ColonyActionType.Mining:
                    if (colonyMiningAction.GetValidActionGridPositionList(colonyTask).Count == 0) continue;
                    GridPosition validActionGridPosition = colonyMiningAction.GetValidActionGridPositionList(colonyTask)[0];
                    colonyMiningAction.TakeAction(_gridPosition, validActionGridPosition, OnActionComplete, colonyTask);
                    colonyTask.AssignedColonist = this;
                    isBusy = true;
                    return;
                case ColonyActionType.Building:
                    if (colonyMiningAction.GetValidActionGridPositionList(colonyTask).Count == 0) continue;
                    GridPosition validActionGridPosition1 = colonyMiningAction.GetValidActionGridPositionList(colonyTask)[0];
                    colonyBuildingAction.TakeAction(_gridPosition, validActionGridPosition1, OnActionComplete, colonyTask);
                    colonyTask.AssignedColonist = this;
                    isBusy = true;
                    return;
            }
        }
        
        if (!isBusy)
        {
            colonyWanderAction.TakeAction(_gridPosition, _gridPosition, OnActionComplete, null);
            isBusy = true;
        }
    }
    
    private void OnActionComplete()
    {
        isBusy = false;
    }
}
