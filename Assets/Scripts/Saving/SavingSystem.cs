using InventorySystem.Saving;
using UnityEngine;

namespace Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save()
        {
            CaptureState();
        }
        
        public void Delete()
        {
            
        }
        
        public void Load()
        {
            RestoreState();
        }

        
        private void CaptureState()
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.CaptureState();
            }
        }

        private void RestoreState()
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
               saveable.RestoreState();
            }
        }
    }
}