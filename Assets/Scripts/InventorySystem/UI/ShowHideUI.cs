using UnityEngine;

namespace GameDevTV.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] private GameObject uiContainer;

        private InputManager _inputManager;
        
        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
            _inputManager = InputManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            if (_inputManager.IsInventoryButtonPressedThisFrame())
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
        }
    }
}