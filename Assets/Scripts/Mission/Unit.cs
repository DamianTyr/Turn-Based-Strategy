using System;
using Grid;
using UnityEngine;

namespace Mission
{
    public class Unit : MonoBehaviour, IDamageable
    {
        private const int ActionPointsMax = 3;
    
        public static event EventHandler OnAnyActionPointChange;
        public static event EventHandler OnAnyUnitSpawned;
        public static event EventHandler OnAnyUnitDead;

        [SerializeField] private bool isEnemy;
    
        private GridPosition _gridPosition;
        
        private int _actionPoints = 3;
        private ActionHolder _actionHolder;
        private HealthSystem _healthSystem;
        
        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _actionHolder = GetComponent<ActionHolder>();
        }

        private void Start()
        {
            _gridPosition = MissionGrid.Instance.GetGridPosition(transform.position);
            MissionGrid.Instance.AddOccupantAtGridPosition(_gridPosition, transform);
        
            TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange; 
            _healthSystem.OnDead += HealthSystem_OnDead;
            
            OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {
            GridPosition newGridPosition = MissionGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition != _gridPosition)
            {
                GridPosition oldGridPosition = _gridPosition;
                _gridPosition = newGridPosition;
                MissionGrid.Instance.OccupantMovedGridPosition(transform, oldGridPosition, newGridPosition);
            }
        }
        
        private void HealthSystem_OnDead(object sender, UnityEngine.Transform damageDealerTransform)
        {
            MissionGrid.Instance.RemoveOccupantAtGridPosition(_gridPosition, transform);
            Destroy(gameObject);

            OnAnyUnitDead(this, EventArgs.Empty);
        }

        private void TurnSystem_OnTurnChange(object sender, EventArgs e)
        {
            if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
                (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
            {
                _actionPoints = ActionPointsMax;
                OnAnyActionPointChange(this, EventArgs.Empty);
            }
        }
        
        public GridPosition GetGridPosition()
        {
            return _gridPosition;
        }
        
        public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (CanSpendActionPointsToTakeAction(baseAction))
            {
                SpendActionPoints(baseAction.GetCost());
                return true;
            }
            return false;
        }

        public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
        {
            return _actionPoints >= baseAction.GetCost();
        }

        private void SpendActionPoints(int amount)
        {
            _actionPoints -= amount;
            OnAnyActionPointChange(this, EventArgs.Empty);
        }

        public int GetActionPoints()
        {
            return _actionPoints;
        }

        public bool IsEnemy()
        {
            return isEnemy;
        }

        public void TakeDamage(int damageAmount, Transform damageDealerTransform)
        {
            _healthSystem.Damage(damageAmount, damageDealerTransform);
        }

        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }

        public float GetHealthNormalized()
        {
            return _healthSystem.GetHealthNormalized();
        }

        public ActionHolder GetActonHolder()
        {
            return _actionHolder;
        }
    }
}
