using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// An slot for the players equipment.
    /// </summary>
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA

        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] EquipLocation equipLocation = EquipLocation.Weapon;

        // CACHE
        private Equipment _selectedEquipment;
        private UnitActionSystem _unitActionSystem;

        // LIFECYCLE METHODS
       
        private void Awake() 
        {
            //var player = GameObject.FindGameObjectWithTag("Player");
            //_selectedEquipment = player.GetComponent<Equipment>();
            
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

        // PUBLIC
        
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

        // PRIVATE

        void RedrawUI()
        {
            icon.SetItem(_selectedEquipment.GetItemInSlot(equipLocation));
        }
    }
}