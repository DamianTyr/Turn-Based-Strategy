using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using UnityEngine;

public class Unit : MonoBehaviour, IDamageable
{
    private const int ActionPointsMax = 9;
    
    public static event EventHandler OnAnyActionPointChange;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    public static event EventHandler OnAnyActionListChanged;

    [SerializeField] private bool isEnemy;
    
    private GridPosition _gridPosition;
    
    private List<BaseAction> _baseActionList;
    private int _actionPoints = 9;
    
    private HealthSystem _healthSystem;
    private Equipment _equipment;

    private Dictionary<EquipLocation, EquipableItem> _equippedItemsDict = new Dictionary<EquipLocation, EquipableItem>();
    
    private void Awake()
    {
        UpdateActionList();
        _healthSystem = GetComponent<HealthSystem>();
        _equipment = GetComponent<Equipment>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange; 
        _healthSystem.OnDead += HealthSystem_OnDead;
        _equipment.OnEquipmentUpdated += Equipment_OnOnEquipmentUpdated;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
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
            _equippedItemsDict[equipLocation] = equipableItem;
            _equippedItemsDict[equipLocation].Setup(transform);
        }

        StartCoroutine(UpdateActionListNextFrame());
    }
    
    private void UpdateActionList()
    {
        BaseAction[] baseActionArray = GetComponents<BaseAction>();
        _baseActionList = baseActionArray.ToList();
        OnAnyActionListChanged?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator UpdateActionListNextFrame()
    {
        yield return null;
        BaseAction[] baseActionArray = GetComponents<BaseAction>();
        _baseActionList = baseActionArray.ToList();
        OnAnyActionListChanged?.Invoke(this, EventArgs.Empty);
    }

    private void HealthSystem_OnDead(object sender, Transform damageDealerTransform)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
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
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        return false;

    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return _actionPoints >= baseAction.GetActionPointsCost();
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

    public void Damage(int damageAmount, Transform damageDealerTransform)
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
