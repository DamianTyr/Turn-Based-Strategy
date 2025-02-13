using System;
using Cinemachine;
using PlayerInput;
using UnityEngine;

public class ArmorySlot : MonoBehaviour, IRaycastable
{
    [SerializeField] private Transform armoryColonistPrefab;
    [SerializeField] private CinemachineVirtualCamera slotVirtualCamera;

    public static Action<ArmorySlot,CinemachineVirtualCamera> OnAnyArmorySlotClicked;
    
    public void SpawnArmoryColonist(CharacterData characterData)
    {
        Transform armoryColonistTransform = Instantiate(armoryColonistPrefab, new Vector3(transform.position.x, 0, transform.position.z), UnityEngine.Quaternion.identity);
        armoryColonistTransform.LookAt(slotVirtualCamera.transform);
        Character character = armoryColonistTransform.GetComponent<Character>();
        character.SetCharacterName(characterData.GetName());
    }

    public CursorType GetCursorType()
    {
        return CursorType.None;
    }

    public bool HandleRaycast(MouseInputHandler callingController, RaycastHit hit)
    {
        return true;
    }

    public void HandleRaycastStop()
    {
        
    }

    public void HandleMouseClick()
    {
        OnAnyArmorySlotClicked?.Invoke(this,slotVirtualCamera);
    }
}
