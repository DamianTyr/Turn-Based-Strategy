using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Inventories;
using UnityEngine;

namespace Mission
{
    public class Unit : MonoBehaviour, IDamageable
    {
        private const int ActionPointsMax = 3;
    
        public static event EventHandler OnAnyActionPointChange;
        public static event EventHandler OnAnyUnitSpawned;
        public static event EventHandler OnAnyUnitDead;
        public static event EventHandler OnAnyActionListChanged;

        [SerializeField] private bool isEnemy;
        [SerializeField] private EquipableWeapon _startingWeapon;
    
        private GridPosition _gridPosition;

        private List<BaseAction> _baseActionList = new List<BaseAction>();
        private int _actionPoints = 3;
    
        private HealthSystem _healthSystem;
        private Equipment _equipment;

        private Dictionary<EquipLocation, EquipableItem> _equippedItemsDict = new Dictionary<EquipLocation, EquipableItem>();
    
        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _equipment = GetComponent<Equipment>();
            UpdateActionList();
        }

        private void Start()
        {
            _gridPosition = MissionGrid.Instance.GetGridPosition(transform.position);
            MissionGrid.Instance.AddOccupantAtGridPosition(_gridPosition, transform);
        
            TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange; 
            _healthSystem.OnDead += HealthSystem_OnDead;
            _equipment.OnEquipmentUpdated += Equipment_OnOnEquipmentUpdated;
        
            _startingWeapon.Setup(transform);
            UpdateActionList();
        
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
    
        private void Equipment_OnOnEquipmentUpdated(EquipLocation equipLocation, EquipableItem equipableItem)
        {
            if (equipableItem == null)
            {
                _equippedItemsDict[equipLocation].RemoveFromUnit(this);
                _equippedItemsDict[equipLocation] = null;
            }
            else
            {
                _startingWeapon.RemoveFromUnit(this);
                _equippedItemsDict[equipLocation] = equipableItem;
                _equippedItemsDict[equipLocation].Setup(transform);
            }

            StartCoroutine(UpdateActionListNextFrame(equipLocation));
        }
    
        private void UpdateActionList()
        {
            BaseAction[] baseActionArray = GetComponents<BaseAction>();
            _baseActionList = baseActionArray.ToList();
            OnAnyActionListChanged?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator UpdateActionListNextFrame(EquipLocation equipLocation)
        {
            yield return null;
        
            //if item was not been replaced Setup Starting Weapon
        
            if (_equippedItemsDict[equipLocation] == null) _startingWeapon.Setup(transform); 

            UpdateActionList();
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


        public T GetActiom<T>() where T : BaseAction
        {
            foreach (BaseAction baseAction in _baseActionList)
            {
                if (baseAction is T)
                {
                    return (T)baseAction;
                }
            }
        
            return null;
        }

        public GridPosition GetGridPosition()
        {
            return _gridPosition;
        }

        public List<BaseAction>GetBaseActionList()
        {
            return _baseActionList;
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

        public void Damage(int damageAmount, UnityEngine.Transform damageDealerTransform)
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
    }
}
