using System;
using UnityEngine;

namespace Saving
{
    public class SavingSystem : MonoBehaviour
    {
        [SerializeField] SceneChanger sceneChanger;

        private void Start()
        {
            sceneChanger.onBeforeSceneChange += Save;
            sceneChanger.onAfterSceneChange += Load;
        }

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

        private void OnDisable()
        {
            sceneChanger.onBeforeSceneChange -= Save;
            sceneChanger.onAfterSceneChange -= Load;
        }
    }
}