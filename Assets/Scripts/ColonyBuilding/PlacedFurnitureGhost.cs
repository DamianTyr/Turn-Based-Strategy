using System;
using System.Collections.Generic;
using Colony;
using UnityEngine;

public class PlacedFurnitureGhost : MonoBehaviour
{
    private GridPosition _gridPosition;
    private int _health = 20;

    private FurnitureSO _furnitureSO;
    [SerializeField] private List<GridPosition> _occupiedGridPositionList;
    
    void Start()
    {
        _gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        ColonyGrid.Instance.SetFurnitureGhostAtGridPosition(_gridPosition, this);
        ColonyTasksManager.Instance.RegisterTask(_gridPosition, ColonyActionType.Building);
    }
    
    public void ProgressTask(int progressAmount, Action onTaskCompleted)
    {
        _health -= progressAmount;
        if (_health <= 0)
        {
            Furniture furniture = Instantiate(_furnitureSO.furniture, transform.position, transform.rotation);
            Debug.Log(this._occupiedGridPositionList.Count);
            furniture.Setup(_furnitureSO, _occupiedGridPositionList);
            gameObject.SetActive(false);
            onTaskCompleted();
        }
    }
    
    public void Setup(FurnitureSO furnitureSO, List<GridPosition> occupiedGridPositionList)
    {
        _furnitureSO = furnitureSO;
        _occupiedGridPositionList = new List<GridPosition>(occupiedGridPositionList);
        Debug.Log(_occupiedGridPositionList.Count);
    }
}
