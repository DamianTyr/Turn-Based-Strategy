using System.Collections.Generic;
using Grid;
using UnityEngine;

[RequireComponent(typeof(ColonyGridSystemVisual))]
public class ColonyGrid : MonoBehaviour, IGrid
{
    public static ColonyGrid Instance { get; private set; }
    protected GridSystem<ColonyGridObject> _gridSystem;
    
    [SerializeField] protected int width;
    [SerializeField] protected int height;
    [SerializeField] protected float cellSize;
    [SerializeField] protected Transform gridDebugObjectPrefab;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one Colony Grid!" + transform + " - " + Instance);
            Destroy(gameObject);
        }
        Instance = this;
        
        _gridSystem = new GridSystem<ColonyGridObject>(width, height, cellSize,  CreateGridObject);
        //_gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private ColonyGridObject CreateGridObject(GridSystem<ColonyGridObject> gridSystem, GridPosition gridPosition)
    {
        return new ColonyGridObject(_gridSystem, gridPosition);
    }
    
    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize, this);
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // GridPosition gridPosition = new GridPosition(x, z);
                // Vector3 worldPosition = Instance.GetWorldPosition(gridPosition);
                //
                // float raycastOffsetDistance = 5f;
                //
                // Ray ray = new Ray(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up);
                // if (Physics.Raycast(ray, out RaycastHit hitInfo))
                // {
                //     if (hitInfo.transform.parent == null) continue;
                //     if (hitInfo.transform.parent.TryGetComponent(out IInteractable interactable))
                //     {
                //         interactable.AddToGridPositionList(gridPosition);
                //         _gridSystem.GetGridObject(gridPosition).SetInteractable(interactable);                   
                //     }
                // }
            }
        }
    }
    
    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);
    public int GetWidth() => _gridSystem.GetWidth();
    public int GetHeight() => _gridSystem.GetHeight();

    public GridPosition GetRandomGridPositionInSquare(GridPosition gridPosition)
    {
        for (int i = 0; i < 5; i++)
        {
            GridPosition random = new GridPosition(Random.Range(-2,3), Random.Range(-2,3));

            GridPosition testGridPosition = gridPosition + random;
            if (IsValidGridPosition(testGridPosition) && Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
            {
                return testGridPosition;
            }
        }
        return new GridPosition(5, 5);
    }

    public List<GridPosition> GetSqaureAroundGridPosition(GridPosition gridPosition, int size)
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        
        for (int x = -size; x <= size; x++)
        {
            for (int z = -size; z <= size; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;
                if (IsValidGridPosition(testGridPosition))
                {
                    gridPositions.Add(testGridPosition);
                }
            }
        }

        return gridPositions;
    }
    
    public void AddOccupantAtGridPosition(GridPosition gridPosition, Transform transform)
    {
        ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddOccupant(transform);
    }

    public List<Transform> GetOccupantListAtGridPosition(GridPosition gridPosition)
    {
        ColonyGridObject gridObject= _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetOccupantList();
    }

    public void RemoveOccupantAtGridPosition(GridPosition gridPosition, Transform transform)
    {
        ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveOccupant(transform);
    }

    public void OccupantMovedGridPosition(Transform transform, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveOccupantAtGridPosition(fromGridPosition, transform);
        AddOccupantAtGridPosition(toGridPosition, transform);
    }

    public bool HasAnyOccupantOnGridPosition(GridPosition gridPosition)
    {
        ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyOccupants();
    }
    
    public Transform GetOccupantAtGridPosition(GridPosition gridPosition)
    {
        ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetOccupant();
    }

    public void SetMinableAtPosition(GridPosition gridPosition, Mineable mineable)
    {
        ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetMineable(mineable);
    }

    public Mineable GetMineableAtGridPosition(GridPosition gridPosition)
    {
        ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetMineable();
    }

}
