using System;
using System.Collections.Generic;
using Colony;
using EPOOutline;
using PlayerInput;
using UnityEngine;

public class Furniture : MonoBehaviour, IRaycastable, IColonyActionTarget
{
    public static Action<Furniture, List<GridPosition>> OnAnySpawned;
    
    private FurnitureSO _furnitureSO;
    private List<GridPosition> _occupiedGridPositionList;
    private Outlinable _outlinable;
    private CraftingSpot _craftingSpot;
    
    public Vector3 transformPosition { get; set; }
    
    public void Setup(FurnitureSO furnitureSO, List<GridPosition> occupiedGridPositionList)
    {
        _furnitureSO = furnitureSO;
        _occupiedGridPositionList = new List<GridPosition>(occupiedGridPositionList); 
        OnAnySpawned?.Invoke(this, _occupiedGridPositionList);
        _outlinable = GetComponent<Outlinable>();
        _outlinable.enabled = false;
        _craftingSpot = GetComponentInChildren<CraftingSpot>();
        transformPosition = transform.position;
    }

    public CursorType GetCursorType()
    {
        return CursorType.Building;
    }

    public bool HandleRaycast(MouseInputHandler callingController, RaycastHit hit)
    {
        _outlinable.enabled = true;
        return true;
    }

    public void HandleRaycastStop()
    {
        _outlinable.enabled = false;
    }

    public void HandleMouseClick()
    {
        GridPosition gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        ColonyTasksManager.Instance.RegisterTask(gridPosition, ColonyActionType.Crafting, this);
    }
    
    public void ProgressTask(int progressAmount, Action onTaskCompleted)
    {
        Debug.Log("Progress Task from Furniture");
    }
}
