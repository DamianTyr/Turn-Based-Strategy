using InventorySystem.Inventories;
using UnityEngine;

namespace InventorySystem.UI
{
    public class ShowHideEquipmentUI : MonoBehaviour
    {
        [SerializeField] private GameObject uiContainer;
        private SelectedEquipmentTracker _selectedEquipmentTracker;
        
        void Start()
        {
            _selectedEquipmentTracker = FindObjectOfType<SelectedEquipmentTracker>();
            _selectedEquipmentTracker.OnSelectedEquipmentChanged += OnSelectedEquipmentChanged;
            uiContainer.SetActive(false);
        }

        private void OnSelectedEquipmentChanged(Equipment equipment)
        {
            if (equipment)
            {
                uiContainer.SetActive(true);
                return;
            }
            uiContainer.SetActive(false);
        }

        private void OnDisable()
        {
            _selectedEquipmentTracker.OnSelectedEquipmentChanged -= OnSelectedEquipmentChanged;
        }
    }
}