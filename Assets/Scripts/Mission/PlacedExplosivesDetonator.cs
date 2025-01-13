using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Mission
{
    public class PlacedExplosivesDetonator : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<PlacedExplosive> placedExplosivesList;
    
        private bool _isActive;
        private float _timer;
        private Action _onInteractionComplete;
    
    
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
    
    
        public void Interact(Action OnInteractionComplete)
        {
            _onInteractionComplete = OnInteractionComplete;
            _isActive = true;
            _timer = .5f;
        
            foreach (PlacedExplosive placedExplosive in placedExplosivesList)
            {
                placedExplosive.Explode();
            }
        }

        public void AddToGridPositionList(GridPosition gridPosition)
        {
        
        }
    }
}
