using System;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Saving;

namespace InventorySystem.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();
        
        public static event Action OnAnyEquipmentUpdated;
        public event Action<EquipLocation, EquipableItem> OnEquipmentUpdated;
        
        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return equippedItems[equipLocation];
        }
        
        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            Debug.Assert(item.GetAllowedEquipLocation() == slot);
            equippedItems[slot] = item;
            OnAnyEquipmentUpdated?.Invoke();
            OnEquipmentUpdated?.Invoke(item.GetAllowedEquipLocation(), item);
        }
        
        public void RemoveItem(EquipLocation slot)
        {
            OnEquipmentUpdated?.Invoke(slot, null);
            equippedItems.Remove(slot);
            OnAnyEquipmentUpdated?.Invoke();
        }
        
        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            return equippedItems.Keys;
        }
        
        object ISaveable.CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (var pair in equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.GetItemID();
            }
            return equippedItemsForSerialization;
        }

        void ISaveable.RestoreState(object state)
        {
            equippedItems = new Dictionary<EquipLocation, EquipableItem>();

            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                var item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
                if (item != null)
                {
                    equippedItems[pair.Key] = item;
                }
            }
        }
    }
}