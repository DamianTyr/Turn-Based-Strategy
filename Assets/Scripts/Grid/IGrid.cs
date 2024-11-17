using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public interface IGrid
    {
        public GridPosition GetGridPosition(Vector3 worldPosition);

        public Vector3 GetWorldPosition(GridPosition gridPosition);

        public bool IsValidGridPosition(GridPosition gridPosition);

        public int GetWidth();

        public int GetHeight();

        public GridPosition GetRandomGridPositionInSquare(GridPosition gridPosition);

        public List<GridPosition> GetSqaureAroundGridPosition(GridPosition gridPosition, int size);

        public void AddOccupantAtGridPosition(GridPosition gridPosition, Transform transform);

        public List<Transform> GetOccupantListAtGridPosition(GridPosition gridPosition);

        public void RemoveOccupantAtGridPosition(GridPosition gridPosition, Transform transform);

        public void OccupantMovedGridPosition(Transform transform, GridPosition fromGridPosition,
            GridPosition toGridPosition);

        public bool HasAnyOccupantOnGridPosition(GridPosition gridPosition);

        public Transform GetOccupantAtGridPosition(GridPosition gridPosition);
    }
}