using UnityEngine;

public class WorldMapCamera : MonoBehaviour
{
    [SerializeField] private GameObject worldCameraGameObject;
    private bool isActive;

    private void Start()
    {
        HideWorldCamera();
    }

    private void Update()
    {
        if (!isActive) return;
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();
        float moveSpeed = 10f;
        
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
    
    private void ShowWorldCamera()
    {
        worldCameraGameObject.SetActive(true);
    }

    private void HideWorldCamera()
    {
        worldCameraGameObject.SetActive(false);
    }

    public void ToggleWorldMapCamera()
    {
        if (isActive)
        {
            HideWorldCamera();
            isActive = false;
            return;
        }
        ShowWorldCamera();
        isActive = true;
    }
}
