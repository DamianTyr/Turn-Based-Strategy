using System;
using System.Collections.Generic;
using UnityEngine;
using Saving;

namespace InventorySystem.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        Dictionary<EquipLocation, EquipableItem> _equippedItems = new();
        
        public static event Action OnAnyEquipmentUpdated;
        public event Action<EquipLocation, EquipableItem> OnEquipmentUpdated;
        
        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!_equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }
            return _equippedItems[equipLocation];
        }
        
        public void AddItem(EquipLocation equipLocation, EquipableItem item)
        {
            Debug.Assert(item.GetAllowedEquipLocation() == equipLocation);
            _equippedItems[equipLocation] = item;
            OnAnyEquipmentUpdated?.Invoke();
            OnEquipmentUpdated?.Invoke(item.GetAllowedEquipLocation(), item);
        }
        
        public void RemoveItem(EquipLocation slot)
        {
            OnEquipmentUpdated?.Invoke(slot, null);
            _equippedItems.Remove(slot);
            OnAnyEquipmentUpdated?.Invoke();
        }
        
        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            return _equippedItems.Keys;
        }
        
        public void CaptureState(string guid)
        {
           
        }

        public void RestoreState(string guid)
        {
            
        }
    }
}