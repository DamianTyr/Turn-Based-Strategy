using System;
using UnityEngine;
using InventorySystem.Inventories;

namespace InventorySystem.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventorySlotUI InventoryItemPrefab = null;
        
        private Inventory _inventory;
        
        private void Awake()
        {
            // _inventory = FindObjectOfType<Inventory>();
            // _inventory.inventoryUpdated += Redraw;
        }

        private void Start()
        {
            _inventory = FindObjectOfType<Inventory>();
            _inventory.inventoryUpdated += Redraw;
            Redraw();
        }
        
        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _inventory.GetSize(); i++)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
                itemUI.Setup(_inventory, i);
            }
        }

        private void OnDestroy()
        {
            _inventory.inventoryUpdated -= Redraw;
        }
    }
}