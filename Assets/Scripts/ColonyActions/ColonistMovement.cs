using System;
using System.Collections.Generic;
using Animancer;
using Colony;
using Grid;
using UnityEngine;

namespace ColonyActions
{
    public class ColonistMovement: MonoBehaviour
    {
        [SerializeField] private AnimationClip idleAnimationClip;
        [SerializeField] private AnimationClip runAnimationClip;
    
        private List<Vector3> _positionList;
        private int _currentPositionIndex;
    
        private ColonyGrid _baseGrid;
    
        protected bool isActive;
        private Action _onMovementComplete;
    
        protected AnimancerComponent animancerComponent;

        private void Awake()
        {
            animancerComponent = GetComponent<AnimancerComponent>();
        }

        private void Start()
        {
            animancerComponent.Play(idleAnimationClip);
            _baseGrid = FindObjectOfType<ColonyGrid>();
        }

        void Update()
        {
            if (!isActive) return;

            Vector3 targetPosition = _positionList[_currentPositionIndex];
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        
            float stoppingDistance = 0.1f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                float moveSpeed = 4f;
                transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            }
            else
            {
                _currentPositionIndex++;
                if (_currentPositionIndex >= _positionList.Count)
                {
                    animancerComponent.Play(idleAnimationClip, .3f);
                    MoveComplete();
                }
            }
        }
    
        public void Move(GridPosition startGridPosition, GridPosition endGridPosition, Action onActionComplete)
        {
            List<GridPosition> pathGridPostionList = Pathfinding.Instance.FindPath(startGridPosition, endGridPosition, out int pathLenght);
            if (pathGridPostionList == null)
            {
                Debug.Log("Aborted Action, Path count 0");
                MoveComplete();
                return;
            }

            _currentPositionIndex = 0;
            _positionList = new List<Vector3>();
        
            foreach (GridPosition pathGridPosition in pathGridPostionList)
            {
                _positionList.Add(_baseGrid.GetWorldPosition(pathGridPosition));
            }
        
            animancerComponent.Play(runAnimationClip, 0.3f);
            MoveStart(onActionComplete);
        }
    
        protected void MoveStart(Action onMovementComplete)
        {
            isActive = true;
            _onMovementComplete = onMovementComplete;
        }

        protected void MoveComplete()
        {
            isActive = false;
            _onMovementComplete();
        }
    }
}
