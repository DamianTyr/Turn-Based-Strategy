using System;
using System.Collections.Generic;
using ColonyBuilding;
using Grid;
using Saving;
using UnityEngine;

namespace Colony
{
    [RequireComponent(typeof(ColonyGridSystemVisual))]
    public class ColonyGrid : MonoBehaviour, IGrid, ISaveable
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
            Mineable.OnAnySpawned += SetMinableAtPosition;
        }
    
        public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
        public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
        public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);
        public int GetWidth() => _gridSystem.GetWidth();
        public int GetHeight() => _gridSystem.GetHeight();
    
        public List<GridPosition> GetSquareAroundGridPosition(GridPosition gridPosition, int size)
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
    
        public List<GridPosition> GetRectangleOfSizeGridPositions(GridPosition center, Vector2Int dimensions)
        {
            int startX = center.X - (dimensions.x > 0 ? 0 : Math.Abs(dimensions.x) -1);
            int endX = center.X + (dimensions.x > 0 ? dimensions.x - 1 : 0);
            int startZ = center.Z - (dimensions.y > 0 ? 0 : Math.Abs(dimensions.y) -1);
            int endZ = center.Z + (dimensions.y > 0 ? dimensions.y - 1 : 0);
        
            List<GridPosition> gridPositionList = new List<GridPosition>();
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startZ; y <= endZ; y++)
                {
                    gridPositionList.Add(new GridPosition(x, y));
                }
            }
            return gridPositionList;
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

        public void ReserveActionSpot(GridPosition gridPosition)
        {
            ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.SetReserved(true);
        }

        public void RemoveReserveActionSpot(GridPosition gridPosition)
        {
            ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.SetReserved(false);
        }

        public bool GetIsReservedAtGridPosition(GridPosition gridPosition)
        {
            ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.GetIsReseved();
        }

        public PlacedFurnitureGhost GetFurnitureGhostAtGridPosition(GridPosition gridPosition)
        {
            ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.GetFurnitureGhost();
        }

        public void SetFurnitureGhostAtGridPosition(GridPosition gridPosition, PlacedFurnitureGhost furnitureGhost)
        {
            ColonyGridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.SetFurnitureGhost(furnitureGhost);
        }
    
        public void CaptureState(string guid)
        {
            _gridSystem.Save();
        }

        public void RestoreState(string guid)
        {

        }
    }
}
