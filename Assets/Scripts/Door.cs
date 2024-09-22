using System;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isOpen;
    private Action _onInteractionComplete;
    private GridPosition _gridPosition;
    private List<GridPosition> _gridPositionList;
    
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
        _gridPositionList = new List<GridPosition>();
    }

    private void Start()
    {
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

    public void AddToGridPositionList(GridPosition gridPosition)
    {
        _gridPositionList.Add(gridPosition);
    }

    public void OpenDoor()
    {
        _isOpen = true;
        _animator.SetBool("IsOpen", _isOpen);
        GetComponentInChildren<BoxCollider>().enabled = false;

        foreach (GridPosition gridPosition in _gridPositionList)
        {
            Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
        }
    }

    public void CloseDoor()
    {
        _isOpen = false;
        _animator.SetBool("IsOpen", _isOpen);
        GetComponentInChildren<BoxCollider>().enabled = true;
        
        foreach (GridPosition gridPosition in _gridPositionList)
        {
            Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
        }
    }
}
