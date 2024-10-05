using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    //public event EventHandler OnAnyUnitMovedGridPosition;
    
    private GridSystem<GridObject> _gridSystem;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    
    [SerializeField] private Transform gridDebugObjectPrefab;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one LevelGrid!" + transform + " - " + Instance);
            Destroy(gameObject);
        }
        Instance = this;
        
        _gridSystem = new GridSystem<GridObject>(width, height, cellSize,  CreateGridObject);
        //_gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

                float raycastOffsetDistance = 5f;

                Ray ray = new Ray(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.transform.parent == null) continue;
                    if (hitInfo.transform.parent.TryGetComponent(out IInteractable interactable))
                    {
                        interactable.AddToGridPositionList(gridPosition);
                        _gridSystem.GetGridObject(gridPosition).SetInteractable(interactable);                   
                    }
                }
            }
        }
    }

    private GridObject CreateGridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        return new GridObject(gridSystem, gridPosition);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }
    
    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);
    public int GetWidht() => _gridSystem.GetWidth();
    public int GetHeight() => _gridSystem.GetHeight();
    
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }
    
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }
}
