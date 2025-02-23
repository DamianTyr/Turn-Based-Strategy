﻿using System;
using UnityEngine;
using Saving;

namespace InventorySystem.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        [SerializeField] private SceneChanger _sceneChanger;
        [Tooltip("Allowed size")]
        [SerializeField] int inventorySize = 16;

        [SerializeField] private EquipableItem testItem;
        
        InventorySlot[] slots;

        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }
        
        public event Action inventoryUpdated;

        private void Start()
        {
            AddToFirstEmptySlot(testItem, 1);
            _sceneChanger.onBeforeSceneChange += SceneChangerOnonBeforeSceneChange;
        }

        private void SceneChangerOnonBeforeSceneChange()
        {
            inventoryUpdated = null;
        }
        
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }
        
        public int GetSize()
        {
            return slots.Length;
        }
        
        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            slots[i].item = item;
            slots[i].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

       
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return true;
                }
            }
            return false;
        }

        
        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot].item;
        }

       
        public int GetNumberInSlot(int slot)
        {
            return slots[slot].number;
        }

        
        public void RemoveFromSlot(int slot, int number)
        {
            slots[slot].number -= number;
            if (slots[slot].number <= 0)
            {
                slots[slot].number = 0;
                slots[slot].item = null;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }
        
        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, number); ;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            slots[slot].item = item;
            slots[slot].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        // PRIVATE

        private void Awake()
        {
            slots = new InventorySlot[inventorySize];
        }
        
        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }
        
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }
        
        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void TestInventorySave()
        {
            ES3.Save("InventoryTest", slots);
        }

        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }
    
        public void CaptureState(string guid)
        {
            
        }

        public void RestoreState(string guid)
        {
           
        }
    }
}