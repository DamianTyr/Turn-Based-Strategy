using System;
using Grid;
using InventorySystem.Inventories;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mission
{
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; } 
     
        public event EventHandler OnSelectedUnitChanged;
        public event EventHandler OnSelectedActionChanged;
        public event EventHandler<bool> OnBusyChange;
        public event EventHandler OnActionStarted;
    
        [SerializeField] private Unit selectedUnit;
        [SerializeField] private LayerMask unitLayerMask;
    
        private BaseAction _selectedAction; 
        private bool _isBusy;

        private SelectedEquipmentTracker _selectedEquipmentTracker;
    
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more then one action system!" + transform + " - " + Instance);
                Destroy(gameObject);
            }
            else Instance = this;
        }

        private void Start()
        {
            _selectedEquipmentTracker = FindObjectOfType<SelectedEquipmentTracker>();
            SetSelectedUnit(selectedUnit);
        }

        void Update()
        {
            if (_isBusy) return;
            if (!TurnSystem.Instance.IsPlayerTurn()) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (TryHandleUnitSelection()) return;
            HandleSelectedAction();
        }

        private void HandleSelectedAction()
        {
            if (InputManager.Instance.IsMouseButtonDownThisFrame())
            {
                GridPosition mouseGridPosition = MissionGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
                if (!selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)) return;
            
                SetBusy();
                _selectedAction.TakeAction(selectedUnit.GetGridPosition(),mouseGridPosition, ClearBusy);
                OnActionStarted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SetBusy()
        {
            _isBusy = true;
            OnBusyChange?.Invoke(this, _isBusy);
        }

        private void ClearBusy()
        {
            _isBusy = false;
            OnBusyChange?.Invoke(this, _isBusy);
        }

        private bool TryHandleUnitSelection()
        {
            if (InputManager.Instance.IsMouseButtonDownThisFrame())
            {
                Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
                if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, unitLayerMask))
                {
                    if (hitInfo.collider.TryGetComponent(out Unit unit))
                    {
                        if (unit == selectedUnit) return false;
                        if (unit.IsEnemy()) return false;
                        SetSelectedUnit(unit);
                        return true;
                    }
                }
            }
            return false;
        }

        private void SetSelectedUnit(Unit unit)
        {
            selectedUnit = unit;
            ActionHolder actionHolder = unit.GetActonHolder();
            SetSelectedAction(actionHolder.GetAction<MoveAction>()); 
            _selectedEquipmentTracker.SetSelectedEquipment(unit.GetComponent<Equipment>());
            OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetSelectedAction(BaseAction baseAction)
        {
            _selectedAction = baseAction;
            Debug.Log(_selectedAction);
            OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        }

        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }

        public BaseAction GetSelectedAction()
        {
            return _selectedAction;
        }
    }
}
