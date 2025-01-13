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

        private SelectedEquipmentTracker _selectedEquipmentTracker;
        
        private void Start()
        {
            _selectedEquipmentTracker = FindObjectOfType<SelectedEquipmentTracker>();
            _selectedEquipmentTracker.OnSelectedEquipmentChanged += OnSelectedEquipmentChanged;
            Equipment.OnAnyEquipmentUpdated += RedrawUI;
            RedrawUI();
        }
        
        private void OnSelectedEquipmentChanged(Equipment selectedEquipment)
        {
            _selectedEquipment = selectedEquipment;
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
            if (_selectedEquipment == null) return null;
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
            if (_selectedEquipment == null) return;
            icon.SetItem(_selectedEquipment.GetItemInSlot(equipLocation));
        }

        private void OnDestroy()
        {
            _selectedEquipmentTracker.OnSelectedEquipmentChanged -= OnSelectedEquipmentChanged;
            Equipment.OnAnyEquipmentUpdated -= RedrawUI;
        }
    }
}