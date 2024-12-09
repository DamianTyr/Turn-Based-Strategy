using System;
using System.Collections.Generic;
using EPOOutline;
using PlayerInput;
using UnityEngine;

public class Furniture : MonoBehaviour, IRaycastable
{
    public static Action<Furniture, List<GridPosition>> OnAnySpawned;
    
    private FurnitureSO _furnitureSO;
    private List<GridPosition> _occupiedGridPositionList;
    private Outlinable _outlinable;
    
    public void Setup(FurnitureSO furnitureSO, List<GridPosition> occupiedGridPositionList)
    {
        _furnitureSO = furnitureSO;
        _occupiedGridPositionList = new List<GridPosition>(occupiedGridPositionList); 
        OnAnySpawned?.Invoke(this, _occupiedGridPositionList);
        _outlinable = GetComponent<Outlinable>();
        _outlinable.enabled = false;
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
        Debug.Log("Furniture Clicked");
    }
}
