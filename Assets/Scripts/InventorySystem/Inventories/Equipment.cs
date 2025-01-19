using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Inventories
{
    public class Equipment : MonoBehaviour
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
            OnEquipmentUpdated?.Invoke(item.GetAllowedEquipLocation(), item);
            OnAnyEquipmentUpdated?.Invoke();
        }
        
        public void RemoveItem(EquipLocation slot)
        {
            _equippedItems.Remove(slot);
            OnEquipmentUpdated?.Invoke(slot, null);
            OnAnyEquipmentUpdated?.Invoke();
        }
        
        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            return _equippedItems.Keys;
        }

        public void Save(string characterName)
        {
            Dictionary<EquipLocation, string> equipedItemIDs = new Dictionary<EquipLocation, string>();
            foreach (EquipLocation equipLocation in _equippedItems.Keys)
            {
                string itemId = _equippedItems[equipLocation].GetItemID();
                equipedItemIDs[equipLocation] = itemId;
            }
            ES3.Save(characterName, equipedItemIDs);
        }

        public void Load(string characterName)
        {
            Dictionary<EquipLocation, string> equipedItemIDs = new Dictionary<EquipLocation, string>();
            equipedItemIDs = ES3.Load<Dictionary<EquipLocation, string>>(characterName);
            foreach (EquipLocation equipLocation in equipedItemIDs.Keys)
            {
                InventoryItem item = InventoryItem.GetFromID(equipedItemIDs[equipLocation]);
                AddItem(equipLocation,(EquipableItem) item);
            }
        }
    }
}