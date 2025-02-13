using UnityEngine;

namespace InventorySystem.UI
{
    public class ShowHideEquipmentUI : MonoBehaviour
    {
        [SerializeField] private GameObject uiContainer;
        private CharacterManager _characterManager;
        
        void Start()
        {
            _characterManager = FindObjectOfType<CharacterManager>();
            _characterManager.OnSelectedCharacterSet += OnCharacterSelectedSet;
            
            uiContainer.SetActive(false);
        }

        private void OnCharacterSelectedSet(Character character)
        {
            if (character)
            {
                uiContainer.SetActive(true);
                return;
            }
            uiContainer.SetActive(false);
        }
        
        private void OnDisable()
        {
            _characterManager.OnSelectedCharacterSet -= OnCharacterSelectedSet;
        }
    }
}