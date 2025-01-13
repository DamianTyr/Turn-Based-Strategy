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
        private GridPosition _mouseGridPosition;
    
        private Vector2Int _baseDimensions;
        public Vector2Int _currentDimensions;
        private List<GridPosition> _occupiedGridPostionList = new List<GridPosition>();
    
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
            if (InputManager.Instance.IsMouseButtonDownThisFrame())
            {
                PlacedFurnitureGhost placedFurnitureGhost = Instantiate(furnitureSO.placedFurnitureGhost, ColonyGrid.Instance.GetWorldPosition(_mouseGridPosition),
                    _spawnedGhostTransform.rotation);
                placedFurnitureGhost.Setup(furnitureSO, _occupiedGridPostionList);
            }
        }

        private void HandleRotation()
        {
            if (InputManager.Instance.IsColonyRotateButtonDownThisFrame())
            {
                RotateGhost();
            }
        }

        private void RotateGhost()
        {
            _rotationIndex = (_rotationIndex + 1) % 4;
            _currentDimensions = GetDimensionsForRotation();
        
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
            if (newGridPosition != _mouseGridPosition)
            {
                _mouseGridPosition = newGridPosition;
                _spawnedGhostTransform.position = ColonyGrid.Instance.GetWorldPosition(_mouseGridPosition);
                UpdateOccupiedGridPositionList();
            }
        }

        private void HandleIsActiveToggle()
        {
            if (InputManager.Instance.IsBuildingButtonDownThisFrame())
            {
                if (_isActive)
                {
                    _isActive = false;
                    Destroy(_spawnedGhostTransform.gameObject);
                    _spawnedGhostTransform = null;
                    OnPlacingFurnitureDisabled?.Invoke();
                }
                else
                {
                    _mouseGridPosition = ColonyGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                    _spawnedGhostTransform = Instantiate(furnitureSO.furnitureGhost, ColonyGrid.Instance.GetWorldPosition(_mouseGridPosition), Quaternion.identity);
                    _baseDimensions = furnitureSO.dimensions;
                    _currentDimensions = _baseDimensions;
                
                    UpdateOccupiedGridPositionList();
                    _rotationIndex = 0;
                    _isActive = true;
                }
            }
        }

        private void UpdateOccupiedGridPositionList()
        {
            _occupiedGridPostionList.Clear();
            _occupiedGridPostionList = ColonyGrid.Instance.GetRectangleOfSizeGridPositions(_mouseGridPosition, _currentDimensions);
            OnAnyGhostManipulated?.Invoke(_occupiedGridPostionList);
        }
    }
}
