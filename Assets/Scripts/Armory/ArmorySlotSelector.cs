using System;
using Cinemachine;
using UnityEngine;

public class ArmorySlotSelector : MonoBehaviour
{
    private ArmorySlot _selectedArmorySlot;
    private ArmoryCameraController _armoryCameraController;
    public Action<ArmorySlot> OnSelectedArmorySlotChanged;
    
    private void Start()
    {
        _armoryCameraController = FindObjectOfType<ArmoryCameraController>();
        ArmorySlot.OnAnyArmorySlotClicked += OnAnyArmorySlotClicked;
    }
    
    private void OnAnyArmorySlotClicked(ArmorySlot armorySlot, CinemachineVirtualCamera cinemachineCamera)
    {
        _selectedArmorySlot = armorySlot;
        _armoryCameraController.SetCameraToArmorySlot(cinemachineCamera);
        _selectedArmorySlot = armorySlot;
        OnSelectedArmorySlotChanged?.Invoke(_selectedArmorySlot);
    }
    
    private void Update()
    {
        CheckForInput();
    }
    
    void CheckForInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (_selectedArmorySlot == null) return;
            _selectedArmorySlot = null;
            _armoryCameraController.SetCameraToSelectionCamera();
            OnSelectedArmorySlotChanged?.Invoke(_selectedArmorySlot);
        }
    }

    public void SpawnArmoryColonistInSelectedSlot(CharacterData characterData)
    {
        _selectedArmorySlot.SpawnArmoryColonist(characterData);
    }
}
