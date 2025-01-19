using System;
using InventorySystem.Inventories;
using UnityEngine;

public class SelectedEquipmentTracker : MonoBehaviour
{
    [SerializeField] private SceneChanger _sceneChanger;
    private Equipment _selectedEquipment;
    
    public Action<Equipment> OnSelectedEquipmentChanged;

    private void Start()
    {
        _sceneChanger.onBeforeSceneChange += SceneChangerOnBeforeSceneChange;
    }

    private void SceneChangerOnBeforeSceneChange()
    {
        _selectedEquipment = null;
        OnSelectedEquipmentChanged = null;
    }

    public void SetSelectedEquipment(Equipment equipment)
    {
        _selectedEquipment = equipment;
        OnSelectedEquipmentChanged?.Invoke(_selectedEquipment);
    }

    public Equipment GetSelectedEquipment()
    {
        return _selectedEquipment;
    }
}
