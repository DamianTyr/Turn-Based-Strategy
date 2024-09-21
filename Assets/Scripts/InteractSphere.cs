using System;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private MeshRenderer meshRenderer;

    private GridPosition _gridPosition;
    private bool _isGreen;
    private bool _isActive;
    private Action _onInteractionComplete;
    private float _timer;
    
    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);
        SetColorGreen();;
    }

    private void Update()
    {
        if (!_isActive) return;
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _isActive = false;
            _onInteractionComplete();
        }
    }

    private void SetColorGreen()
    {
        _isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    private void SetColorRed()
    {
        _isGreen = false;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action OnInteractionComplete)
    {
        _onInteractionComplete = OnInteractionComplete;
        _isActive = true;
        _timer = .5f;
        
        if (_isGreen) SetColorRed();
        else SetColorGreen();
    }
}
