using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInputActions _playerInputActions;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more then one Input Manager!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }


    public Vector2 GetMouseScreenPosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public bool IsMouseButtonDownThisFrame()
    {
        return _playerInputActions.Player.Click.WasPressedThisFrame();
    }

    public Vector2 GetCameraMoveVector()
    {
        return _playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
    }

    public float GetCameraRotateAmount()
    {
        return _playerInputActions.Player.CameraRotate.ReadValue<float>();
    }

    public float GetCameraZoomAmount()
    {
        return _playerInputActions.Player.CameraZoom.ReadValue<float>();
    }
    
    public bool IsInventoryButtonPressedThisFrame()
    {
        return _playerInputActions.Player.Inventory.WasPressedThisFrame();
    }
}
