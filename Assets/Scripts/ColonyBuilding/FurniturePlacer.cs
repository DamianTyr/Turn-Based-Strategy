using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePlacer : MonoBehaviour
{
    [SerializeField] private Transform furnitureTransform;
    [SerializeField] private Transform testGhostTransform;
    [SerializeField] private Transform testPlacedGhostTransform;
    
    
    [SerializeField] private Transform selectedFurnitureGhost;
    private bool _isActive = false;

    private GridPosition mouseGridPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void Update()
    {
        CheckForToggle();
        if (!_isActive) return;
        HandleMouseMovement();

        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            Instantiate(testPlacedGhostTransform, ColonyGrid.Instance.GetWorldPosition(mouseGridPosition), Quaternion.identity);
        }
    }

    private void HandleMouseMovement()
    {
        GridPosition newGridPosition = ColonyGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        if (newGridPosition != mouseGridPosition)
        {
            mouseGridPosition = newGridPosition;
        }

        selectedFurnitureGhost.position = ColonyGrid.Instance.GetWorldPosition(mouseGridPosition);
    }

    private void CheckForToggle()
    {
        if (InputManager.Instance.IsBuildingButtonPressedThisFrame())
        {
            if (_isActive)
            {
                _isActive = false;
                Destroy(selectedFurnitureGhost.gameObject);
                selectedFurnitureGhost = null;
            }
            else
            {
                mouseGridPosition = ColonyGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                selectedFurnitureGhost = Instantiate(testGhostTransform, ColonyGrid.Instance.GetWorldPosition(mouseGridPosition), Quaternion.identity);
                _isActive = true;
            }
        }
    }
}
