using System;
using UnityEngine;
using Mission;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;
    [SerializeField] private GameObject inventoryCameraGameObject;

    private InputManager _inputManager;
    private bool _isShowingInvetory;
    
    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnOnAnyActionStarted; 
        BaseAction.OnAnyActionCompleted += BaseActionOnOnAnyActionCompleted;
        
        HideActionCamera();
        HideInventoryCamera();
        _inputManager = InputManager.Instance;
    }

    private void Update()
    {
        if (_inputManager.IsInventoryButtonDownThisFrame())
        {
            if (_isShowingInvetory)
            {
                HideInventoryCamera();
                _isShowingInvetory = false;
            }
            else
            {
                ShowInventoryCamera();
                _isShowingInvetory = true;
            }
        }
    }

    private void BaseActionOnOnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    private void BaseAction_OnOnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                UnityEngine.Transform shooterTransform = shootAction.GetHolderTransform();
                Unit targetUnit = shootAction.GetTargetUnit();
                
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                Vector3 shootDirection = (targetUnit.GetWorldPosition() - shooterTransform.position).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shootAction.GetHolderTransform().position + cameraCharacterHeight + shoulderOffset + (shootDirection * -1f);

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                
                ShowActionCamera();
                break;
        }
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    private void ShowInventoryCamera()
    {
        inventoryCameraGameObject.SetActive(true);
    }
    
    private void HideInventoryCamera()
    {
        inventoryCameraGameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BaseAction.OnAnyActionStarted -= BaseAction_OnOnAnyActionStarted; 
        BaseAction.OnAnyActionCompleted -= BaseActionOnOnAnyActionCompleted;
    }
}
