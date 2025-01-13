using System;
using InventorySystem.Inventories;
using UnityEngine;

public class SelectedEquipmentTracker : MonoBehaviour
{
    private Equipment _selectedEquipment;

    public Action<Equipment> OnSelectedEquipmentChanged;
    
    public void SetSelectedEquipment(Equipment equipment)
    {
        _selectedEquipment = equipment;
        OnSelectedEquipmentChanged?.Invoke(_selectedEquipment);
    }
}
