using System;
using System.Collections.Generic;
using InventorySystem.Inventories;
using UnityEngine;

public class EquipmentSetupHandler : MonoBehaviour
{
   [SerializeField] private EquipableWeapon _defaultWeapon;
    
   private Equipment _equipment;
   private Dictionary<EquipLocation, EquipableItem> _equippedItemsDict = new();

   public Action onEquipmentSetup;

   private void Start()
   {
      _equipment = GetComponent<Equipment>();
      _equipment.OnEquipmentUpdated += Equipment_OnEquipmentUpdated;
      _defaultWeapon.Setup(transform);
      onEquipmentSetup?.Invoke();
   }

   private void Equipment_OnEquipmentUpdated(EquipLocation equipLocation, EquipableItem equipableItem)
   {
      if (equipableItem == null)
      {
         _equippedItemsDict[equipLocation].RemoveFromUnit(this);
         _equippedItemsDict[equipLocation] = null;
         if (equipLocation == EquipLocation.Weapon) _defaultWeapon.Setup(transform);
      }
      else
      {
         _defaultWeapon.RemoveFromUnit(this);
         _equippedItemsDict[equipLocation] = equipableItem;
         _equippedItemsDict[equipLocation].Setup(transform);
      }
      onEquipmentSetup?.Invoke();
   }
   
   private void OnDestroy()
   {
      _equipment.OnEquipmentUpdated -= Equipment_OnEquipmentUpdated;
   }
}
