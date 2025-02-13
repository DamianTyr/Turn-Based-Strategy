using InventorySystem.Inventories;
using Saving;
using UnityEngine;

public class Character : MonoBehaviour, ISaveable
{
    [SerializeField] private string characterName;
    private Equipment _characterEquipment;

    private void Start()
    {
        _characterEquipment = GetComponent<Equipment>();
    }
    
    public string GetCharacterName()
    {
        return characterName;
    }

    public void SetCharacterName(string name)
    {
        characterName = name;
    }

    public Equipment GetCharacterEquipment()
    {
        return _characterEquipment;
    }

    public void CaptureState(string guid)
    {
        _characterEquipment.Save(guid);  
    }

    public void RestoreState(string guid)
    {
        _characterEquipment.Load(guid);  
    }
}
