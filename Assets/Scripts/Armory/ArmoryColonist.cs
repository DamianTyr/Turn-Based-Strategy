using InventorySystem.Inventories;
using UnityEngine;

public class ArmoryColonist : MonoBehaviour
{
    private Equipment _equipment;
    
    private void Start()
    {
        _equipment = GetComponent<Equipment>();
    }
    
    public Equipment GetEquipment()
    {
        return _equipment;
    }
}
