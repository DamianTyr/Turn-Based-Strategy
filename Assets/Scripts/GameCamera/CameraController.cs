using Cinemachine;
using UnityEngine;

namespace GameCamera
{
    public class CameraController : MonoBehaviour
    {
        private const float MinFollowYOffset = 2f;
        private const float MaxFollowYOffset = 36f;
    
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private Vector3 targetFollowOfset;
        private CinemachineTransposer cinemachineTransposer;

        private void Start()
        {
            cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            targetFollowOfset = cinemachineTransposer.m_FollowOffset;
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleZoom();
        }

        private void HandleMovement()
        {
            Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();
            float moveSpeed = 10f;
        
            Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }

        private void HandleRotation()
        {
            Vector3 rotationVector = new Vector3(0, 0, 0);
            rotationVector.y = InputManager.Instance.GetCameraRotateAmount();
        
            float rotateSpeed = 100f;
            transform.eulerAngles += rotationVector * (rotateSpeed * Time.deltaTime);
        }

        private void HandleZoom()
        {
            float zoomIncreaseAmount = 1f;
            targetFollowOfset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;
            targetFollowOfset.y = Mathf.Clamp(targetFollowOfset.y, MinFollowYOffset, MaxFollowYOffset);
        
            float zoomSpeed = 5f;
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOfset, zoomSpeed * Time.deltaTime);
        }
    }
}
