using Unity.VisualScripting;
using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    public abstract class EquipableItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;
        
        // PUBLIC

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }

        public abstract void Setup(Transform transform);

        public abstract void RemoveFromUnit(Mission.Unit unit);
    }
}