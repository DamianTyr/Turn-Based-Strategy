using System;
using System.Collections.Generic;
using Colony;
using Grid;
using UnityEngine;

namespace ColonyBuilding
{
    public class FurniturePlacer : MonoBehaviour
    {
        [SerializeField] private FurnitureSO furnitureSO;
    
        public static event Action<List<GridPosition>> OnAnyGhostManipulated;
        public static event Action OnPlacingFurnitureDisabled;
    
        private Transform _spawnedGhostTransform;
        private bool _isActive;
        private bool _isOverValidGridPosition;
        private GridPosition _mouseGridPosition;
    
        private Vector2Int _baseDimensions;
        public Vector2Int _currentDimensions;
        private List<GridPosition> _occupiedGridPostionList = new();
    
        private int _rotationIndex = 0;
    
        void Update()
        {
            HandleIsActiveToggle();
            if (!_isActive) return;
            HandleMouseMovement();
            HandleRotation();
            HandleMouseClick();
        }

        private void HandleMouseClick()
        {
            if (!_isOverValidGridPosition) return;
            if (InputManager.Instance.IsMouseButtonDownThisFrame())
            {
                PlacedFurnitureGhost placedFurnitureGhost = Instantiate(furnitureSO.placedFurnitureGhost, ColonyGrid.Instance.GetWorldPosition(_mouseGridPosition),
                    _spawnedGhostTransform.rotation);
                placedFurnitureGhost.Setup(furnitureSO, _occupiedGridPostionList);
            }
        }

        private void HandleRotation()
        {
            if (!_isOverValidGridPosition) return;
            if (InputManager.Instance.IsColonyRotateButtonDownThisFrame())
            {
                RotateGhost();
            }
        }

        private void RotateGhost()
        {
            _rotationIndex = (_rotationIndex + 1) % 4;
            _currentDimensions = GetDimensionsForRotation();

            if (!IsValidGridPositionRect(_mouseGridPosition))
            {
                HideGhost();
                OnAnyGhostManipulated(null);
                _spawnedGhostTransform.rotation = Quaternion.Euler(0, _rotationIndex * 90, 0);
                return;
            }
            
            ShowGhost();
            UpdateOccupiedGridPositionList();
            _spawnedGhostTransform.rotation = Quaternion.Euler(0, _rotationIndex * 90, 0);
        }
    
        private Vector2Int GetDimensionsForRotation()
        {
            int newX = (_rotationIndex % 2 == 0 ? _baseDimensions.x : _baseDimensions.y);
            int newY = (_rotationIndex % 2 == 0 ? _baseDimensions.y : _baseDimensions.x);

            // Negate based on rotation index
            if (_rotationIndex == 2 || _rotationIndex == 3)
                newX = -newX;
            if (_rotationIndex == 1 || _rotationIndex == 2)
                newY = -newY;

            return new Vector2Int(newX, newY);
        }
    
        private void HandleMouseMovement()
        {
            GridPosition newGridPosition = ColonyGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if (newGridPosition == _mouseGridPosition) return;
            if (!IsValidGridPositionRect(newGridPosition))
            {
                _isOverValidGridPosition = false;
                HideGhost();
                OnAnyGhostManipulated(null);
                return;
            }

            _isOverValidGridPosition = true;
            ShowGhost();
            _mouseGridPosition = newGridPosition;
            _spawnedGhostTransform.position = ColonyGrid.Instance.GetWorldPosition(_mouseGridPosition);
            UpdateOccupiedGridPositionList();
        }

        private void HideGhost()
        {
            if (!_spawnedGhostTransform) return;
            _spawnedGhostTransform.gameObject.SetActive(false);
        }

        private void ShowGhost()
        {
            if (!_spawnedGhostTransform) return;
            _spawnedGhostTransform.gameObject.SetActive(true);
        }

        private void HandleIsActiveToggle()
        {
            if (InputManager.Instance.IsBuildingButtonDownThisFrame())
            {
                if (_isActive)
                {
                    HandleDeactivation();
                    return;
                }

                _mouseGridPosition = ColonyGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                if (!IsValidGridPositionRect(_mouseGridPosition))
                {
                    _spawnedGhostTransform = Instantiate(furnitureSO.furnitureGhost, ColonyGrid.Instance.GetWorldPosition(new GridPosition(1, 1)), Quaternion.identity);
                    SetupGhostInfo();
                    HideGhost();
                }

                _spawnedGhostTransform = Instantiate(furnitureSO.furnitureGhost, ColonyGrid.Instance.GetWorldPosition(_mouseGridPosition), Quaternion.identity);
                SetupGhostInfo();
                UpdateOccupiedGridPositionList();
            }
        }

        private void HandleDeactivation()
        {
            _isActive = false;
            Destroy(_spawnedGhostTransform.gameObject);
            _spawnedGhostTransform = null;
            OnPlacingFurnitureDisabled?.Invoke();
        }

        private void SetupGhostInfo()
        {
            _baseDimensions = furnitureSO.dimensions;
            _currentDimensions = _baseDimensions;
            _rotationIndex = 0;
            _isActive = true;
        }

        private bool IsValidGridPositionRect(GridPosition gridPosition)
        {
            if (!ColonyGrid.Instance.IsValidGridPosition(gridPosition)) return false;
            
            List<GridPosition> gridPositions = ColonyGrid.Instance.GetRectangleOfSizeGridPositions(gridPosition, _currentDimensions);
            foreach (GridPosition gridPos in gridPositions)
            {
                if (!ColonyGrid.Instance.IsValidGridPosition(gridPos)) return false;
            }
            return true;
        }

        private void UpdateOccupiedGridPositionList()
        {
            _occupiedGridPostionList.Clear();
            _occupiedGridPostionList = ColonyGrid.Instance.GetRectangleOfSizeGridPositions(_mouseGridPosition, _currentDimensions);
            OnAnyGhostManipulated?.Invoke(_occupiedGridPostionList);
        }
    }
}
