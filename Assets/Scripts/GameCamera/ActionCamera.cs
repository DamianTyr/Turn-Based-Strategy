using System;
using Mission;
using UnityEngine;

namespace GameCamera
{
    public class ActionCamera : MonoBehaviour
    {
        [SerializeField] private GameObject actionCameraGameObject;
    
        private void Start()
        {
            BaseAction.OnAnyActionStarted += BaseAction_OnOnAnyActionStarted; 
            BaseAction.OnAnyActionCompleted += BaseActionOnOnAnyActionCompleted;
        
            HideActionCamera();
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
                    Transform shooterTransform = shootAction.GetHolderTransform();
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
        
        private void OnDestroy()
        {
            BaseAction.OnAnyActionStarted -= BaseAction_OnOnAnyActionStarted; 
            BaseAction.OnAnyActionCompleted -= BaseActionOnOnAnyActionCompleted;
        }
    }
}
