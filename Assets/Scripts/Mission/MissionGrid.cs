using System.Collections.Generic;
using Mission;
using UnityEngine;

public class MissionGrid : BaseGrid
{
    public static MissionGrid Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one LevelGrid!" + transform + " - " + Instance);
            Destroy(gameObject);
        }
        Instance = this;
        
        _gridSystem = new GridSystem<IGridObject>(width, height, cellSize,  CreateGridObject);
        //_gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }
    
    private IGridObject CreateGridObject(GridSystem<IGridObject> gridSystem, GridPosition gridPosition)
    {
        return new MissionGridObject(gridSystem, gridPosition);
    }
    
    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize, this);
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = Instance.GetWorldPosition(gridPosition);

                float raycastOffsetDistance = 5f;

                Ray ray = new Ray(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.transform.parent == null) continue;
                    if (hitInfo.transform.parent.TryGetComponent(out IInteractable interactable))
                    {
                        interactable.AddToGridPositionList(gridPosition);
                        IGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
                        MissionGridObject missionGridObject = gridObject as MissionGridObject;
                        missionGridObject.SetInteractable(interactable);                   
                    }
                }
            }
        }
    }
    
    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        IGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        MissionGridObject missionGridObject = gridObject as MissionGridObject;
        return missionGridObject.GetInteractable();
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        IGridObject IgridObject = _gridSystem.GetGridObject(gridPosition);
        MissionGridObject missionGridObject = IgridObject as MissionGridObject;
        missionGridObject.SetInteractable(interactable);
    }
}
