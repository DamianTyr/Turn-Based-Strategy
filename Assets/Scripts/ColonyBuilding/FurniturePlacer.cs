using UnityEngine;

public class FurniturePlacer : MonoBehaviour
{
    [SerializeField] private FurnitureSO furnitureSO;
    
    private Transform _spawnedGhostTransform;
    private bool _isActive;
    private GridPosition _mouseGridPosition;
    
    private Vector2 _rotatedDimensions;
    private int _rotationIndex;
    
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
            placedFurnitureGhost.SetFurnitureSO(furnitureSO);
        }
    }

    private void HandleRotation()
    {
        if (InputManager.Instance.IsColonyRotateButtonDownThisFrame())
        {
            _rotationIndex += 1;
            _spawnedGhostTransform.eulerAngles += new Vector3(0, 90, 0);
            if (_spawnedGhostTransform.eulerAngles.y >= 360)
            {
                _spawnedGhostTransform.eulerAngles = new Vector3(0, 0, 0);
            }

            switch (_rotationIndex)
            {
                case 0:
                {
                    _rotatedDimensions = furnitureSO.dimensions;
                    break;
                }
                case 1:
                {
                    _rotatedDimensions = new Vector2(furnitureSO.dimensions.y, -furnitureSO.dimensions.x);
                    break;
                }
                case 2:
                {
                    _rotatedDimensions = new Vector2(-furnitureSO.dimensions.x, furnitureSO.dimensions.y);
                    break;
                }
                case 3:
                {
                    _rotatedDimensions = new Vector2(furnitureSO.dimensions.y, furnitureSO.dimensions.x);
                    break;
                }
            }
            Debug.Log("Rotated dimensions " + _rotatedDimensions);
        }
    }

    private void HandleMouseMovement()
    {
        GridPosition newGridPosition = ColonyGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        if (newGridPosition != _mouseGridPosition)
        {
            _mouseGridPosition = newGridPosition;
            _spawnedGhostTransform.position = ColonyGrid.Instance.GetWorldPosition(_mouseGridPosition);
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
            }
            else
            {
                _mouseGridPosition = ColonyGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                _spawnedGhostTransform = Instantiate(furnitureSO.furnitureGhost, ColonyGrid.Instance.GetWorldPosition(_mouseGridPosition), Quaternion.identity);
                _rotationIndex = 0;
                _isActive = true;
            }
        }
    }
}
