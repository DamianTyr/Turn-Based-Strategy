using System;
using UnityEngine;
using InventorySystem.Core.UI.Dragging;
using InventorySystem.Inventories;
using Mission;

namespace InventorySystem.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon icon;
        [SerializeField] EquipLocation equipLocation = EquipLocation.Weapon;
        
        private Equipment _selectedEquipment;
        private UnitActionSystem _unitActionSystem;
        
        private void Awake() 
        {
            _unitActionSystem = UnitActionSystem.Instance;
            _unitActionSystem.OnSelectedUnitChanged += UnitActionSystem_OnOnSelectedUnitChanged;

            Unit selectedUnit = _unitActionSystem.GetSelectedUnit();
            _selectedEquipment = selectedUnit.GetComponent<Equipment>();
            Equipment.OnAnyEquipmentUpdated += RedrawUI;
        }

        private void UnitActionSystem_OnOnSelectedUnitChanged(object sender, EventArgs e)
        {
            Unit selectedUnit = _unitActionSystem.GetSelectedUnit();
            Equipment selectedEquipment = selectedUnit.GetComponent<Equipment>();

            if (_selectedEquipment == null)
            {
                _selectedEquipment = selectedEquipment;
                RedrawUI();
            }
            _selectedEquipment = selectedEquipment;
            RedrawUI();
        }
        private void Start() 
        {
            RedrawUI();
        }
        
        public int MaxAcceptable(InventoryItem item)
        {
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedEquipLocation() != equipLocation) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }

        public void AddItems(InventoryItem item, int number)
        {
            _selectedEquipment.AddItem(equipLocation, (EquipableItem) item);
        }

        public InventoryItem GetItem()
        {
            return _selectedEquipment.GetItemInSlot(equipLocation);
        }

        public int GetNumber()
        {
            if (GetItem() != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void RemoveItems(int number)
        {
            _selectedEquipment.RemoveItem(equipLocation);
        }
        
        void RedrawUI()
        {
            icon.SetItem(_selectedEquipment.GetItemInSlot(equipLocation));
        }
    }
}