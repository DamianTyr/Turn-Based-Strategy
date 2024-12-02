using System;
using Colony;
using UnityEngine;

public class PlacedFurnitureGhost : MonoBehaviour
{
    public static Action<PlacedFurnitureGhost> OnAnySpawned;
    private GridPosition _gridPosition;
    private int _health = 20;

    private FurnitureSO _furnitureSO;
    
    void Start()
    {
        OnAnySpawned?.Invoke(this);

        _gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        ColonyGrid.Instance.SetFurnitureGhostAtGridPosition(_gridPosition, this);
        ColonyTasksManager.Instance.RegisterTask(_gridPosition, ColonyActionType.Building);
    }
    
    public void ProgressTask(int progressAmount, Action onTaskCompleted)
    {
        _health -= progressAmount;
        if (_health <= 0)
        {
            Instantiate(_furnitureSO.furniture, transform.position, transform.rotation);
            gameObject.SetActive(false);
            onTaskCompleted();
        }
    }

    public void SetFurnitureSO(FurnitureSO furnitureSO)
    {
        _furnitureSO = furnitureSO;
    }
}
