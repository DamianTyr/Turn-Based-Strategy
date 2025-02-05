using InventorySystem.Inventories;
using Saving;
using UnityEngine;

public class Character : MonoBehaviour, ISaveable
{
    [SerializeField] private string characterName;
    private Equipment characterEquipment;

    private void Start()
    {
        characterEquipment = GetComponent<Equipment>();
    }

    public void CaptureState(string guid)
    {
        characterEquipment.Save(guid);  
    }

    public void RestoreState(string guid)
    {
        characterEquipment.Load(guid);  
    }
}
