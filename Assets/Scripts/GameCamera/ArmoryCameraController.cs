using System;
using Cinemachine;
using UnityEngine;

public class ArmoryCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _selectionCamera;
    private CinemachineVirtualCamera currentSlotCamera;
    
    public void SetCameraToArmorySlot(CinemachineVirtualCamera slotCamera)
    {
        _selectionCamera.Priority = 0;
        currentSlotCamera = slotCamera;
        slotCamera.Priority = 10;
    }
    
    public void SetCameraToSelectionCamera()
    {
        _selectionCamera.Priority = 10;
        currentSlotCamera.Priority = 0;
        currentSlotCamera = null;
    }
}