using System;
using UnityEngine;
using InventorySystem.Core.UI.Dragging;
using InventorySystem.Inventories;

namespace InventorySystem.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon icon;
        [SerializeField] EquipLocation equipLocation = EquipLocation.Weapon;
        
        private Equipment _selectedEquipment;
        private CharacterManager _characterManager;
        
        private void Start()
        {
            _characterManager = FindObjectOfType<CharacterManager>();
            _characterManager.OnSelectedCharacterSet += OnCharacterSelected;
            Character selectedCharacter = _characterManager.GetSelectedCharacter();
            if (selectedCharacter)
            {
                _selectedEquipment = selectedCharacter.GetCharacterEquipment();
            }
            
            Equipment.OnAnyEquipmentUpdated += RedrawUI;
            RedrawUI();
        }

        private void OnCharacterSelected(Character character)
        {
            if (!character) return;
            Equipment equipment = character.GetCharacterEquipment();
            _selectedEquipment = equipment;
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

        private void OnDisable()
        {
            Equipment.OnAnyEquipmentUpdated -= RedrawUI;
        }
    }
}