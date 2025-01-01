using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        
        static Dictionary<string, SaveableEntity> _globalLookup = new();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }
        
        public void CaptureState()
        {
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                saveable.CaptureState(uniqueIdentifier);
            }
        }

        public void RestoreState()
        {
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                saveable.RestoreState(uniqueIdentifier);
            }
        }

        // PRIVATE

#if UNITY_EDITOR
        private void Update() {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            _globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!_globalLookup.ContainsKey(candidate)) return true;

            if (_globalLookup[candidate] == this) return true;

            if (_globalLookup[candidate] == null)
            {
                _globalLookup.Remove(candidate);
                return true;
            }

            if (_globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                _globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}