using System;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public static Action<Furniture, List<GridPosition>> OnAnySpawned;
    
    private FurnitureSO _furnitureSO;
    private List<GridPosition> _occupiedGridPositionList;
    
    public void Setup(FurnitureSO furnitureSO, List<GridPosition> occupiedGridPositionList)
    {
        Debug.Log("Furniture Setup, count:" + occupiedGridPositionList.Count);
        _furnitureSO = furnitureSO;
        _occupiedGridPositionList = new List<GridPosition>(occupiedGridPositionList); 
        OnAnySpawned?.Invoke(this, _occupiedGridPositionList);
    }
}
