using System;
using System.Collections.Generic;
using Colony;
using UnityEngine;

public class PlacedFurnitureGhost : MonoBehaviour, IColonyActionTarget
{
    private GridPosition _gridPosition;
    private int _requiredProgress = 20;

    private FurnitureSO _furnitureSO;
    [SerializeField] private List<GridPosition> _occupiedGridPositionList;
    
    public Vector3 transformPosition { get; set; }
    
    void Start()
    {
        _gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        ColonyGrid.Instance.SetFurnitureGhostAtGridPosition(_gridPosition, this);
        ColonyTasksManager.Instance.RegisterTask(_gridPosition, ColonyActionType.Building, this);
        transformPosition = transform.position;
    }
    
    public void ProgressTask(int progressAmount, Action onTaskCompleted)
    {
        _requiredProgress -= progressAmount;
        if (_requiredProgress <= 0)
        {
            Furniture furniture = Instantiate(_furnitureSO.furniture, transform.position, transform.rotation);
            furniture.Setup(_furnitureSO, _occupiedGridPositionList);
            gameObject.SetActive(false);
            onTaskCompleted();
        }
    }
    
    public void Setup(FurnitureSO furnitureSO, List<GridPosition> occupiedGridPositionList)
    {
        _furnitureSO = furnitureSO;
        _occupiedGridPositionList = new List<GridPosition>(occupiedGridPositionList);
    }
}
