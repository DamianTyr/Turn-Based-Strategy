using System;
using UnityEngine;

public class Colonist : MonoBehaviour
{
    private GridPosition _gridPosition;
    [SerializeField] private ColonyMoveAction colonyMoveAction;
    
    private void Start()
    {
        _gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
    }
    
    private void Update()
    {
        GridPosition newGridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            //ColonyGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }
    
    
    public void MoveTo(GridPosition gridPosition, Action onActionComplete)
    {
       colonyMoveAction.TakeAction(_gridPosition, gridPosition, OnActionComplete);
    }

    private void OnActionComplete()
    {
        Debug.Log("Action Complete");
    }

    private GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
    

}
