using UnityEngine;

public class ArmoryColonistSelector : MonoBehaviour
{
    private SelectedEquipmentTracker _selectedEquipmentTracker;
    private ArmoryColonist _selectedArmoryColonist;
    
    private void Start()
    {
        ArmoryColonist.OnAnyArmoryColonnistClicked += OnAnyArmoryColonnistClicked;
        _selectedEquipmentTracker = FindObjectOfType<SelectedEquipmentTracker>();
    }

    private void OnAnyArmoryColonnistClicked(ArmoryColonist armoryColonist)
    {
        _selectedArmoryColonist = armoryColonist; 
        _selectedEquipmentTracker.SetSelectedEquipment(armoryColonist.GetEquipment());
    }
}
