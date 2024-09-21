using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isOpen;
    private Action _onInteractionComplete;
    private GridPosition _gridPosition;
    private Animator _animator;

    private bool _isActive;
    private float _timer;

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
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);

        if (_isOpen)
        {
            OpenDoor();
        }
        else
        {    
            CloseDoor();
        }
    }

    public void Interact(Action OnInteractionComplete)
    {
        _onInteractionComplete = OnInteractionComplete;
        _isActive = true;
        _timer = .5f;
        if (_isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        _isOpen = true;
        _animator.SetBool("IsOpen", _isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, true);
    }

    public void CloseDoor()
    {
        _isOpen = false;
        _animator.SetBool("IsOpen", _isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, false);
    }
}
