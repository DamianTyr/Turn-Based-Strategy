using System;
using UnityEngine;

public class Colonist : MonoBehaviour
{
    private GridPosition _gridPosition;
    [SerializeField] private MoveAction _moveAction;
    [SerializeField] private AnimationClip idleAnimationClip;
    [SerializeField] private AnimationClip runAnimationClip;

    private void Awake()
    {
        _moveAction.SetAnimationClips(idleAnimationClip, runAnimationClip);
    }

    private void Start()
    {
        _gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
    }
    
    public void MoveTo(GridPosition gridPosition, Action onActionComplete)
    {
       _moveAction.TakeAction(_gridPosition, gridPosition, OnActionComplete);
    }

    private void OnActionComplete()
    {
        Debug.Log("Action Complete");
    }

    private GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
}
