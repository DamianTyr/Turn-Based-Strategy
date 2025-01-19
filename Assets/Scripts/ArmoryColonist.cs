using System;
using System.Collections.Generic;
using InventorySystem.Inventories;
using PlayerInput;
using UnityEngine;

public class ArmoryColonist : MonoBehaviour, IRaycastable
{
    private Equipment _equipment;
    private Dictionary<EquipLocation, EquipableItem> _equippedItemsDict = new();

    public static Action<ArmoryColonist> OnAnyArmoryColonnistClicked;

    private void Start()
    {
        _equipment = GetComponent<Equipment>();
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
        OnAnyArmoryColonnistClicked?.Invoke(this);
    }

    public Equipment GetEquipment()
    {
        return _equipment;
    }
}
