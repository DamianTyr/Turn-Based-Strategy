using System.Collections.Generic;
using UnityEngine;

public class BaseGrid : MonoBehaviour
{
    protected GridSystem<IGridObject> _gridSystem;
    
    [SerializeField] protected int width;
    [SerializeField] protected int height;
    [SerializeField] protected float cellSize;
    [SerializeField] protected Transform gridDebugObjectPrefab;
    
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
                if (ColonyGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    gridPositions.Add(testGridPosition);
                }
            }
        }

        return gridPositions;
    }
    
    public void AddOccupantAtGridPosition(GridPosition gridPosition, Transform transform)
    {
        IGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddOccupant(transform);
    }

    public List<Transform> GetOccupantListAtGridPosition(GridPosition gridPosition)
    {
        IGridObject gridObject= _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetOccupantList();
    }

    public void RemoveOccupantAtGridPosition(GridPosition gridPosition, Transform transform)
    {
        IGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveOccupant(transform);
    }

    public void OccupantMovedGridPosition(Transform transform, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveOccupantAtGridPosition(fromGridPosition, transform);
        AddOccupantAtGridPosition(toGridPosition, transform);
    }

    public bool HasAnyOccupantOnGridPosition(GridPosition gridPosition)
    {
        IGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyOccupants();
    }
    
    public Transform GetOccupantAtGridPosition(GridPosition gridPosition)
    {
        IGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetOccupant();
    }
}
