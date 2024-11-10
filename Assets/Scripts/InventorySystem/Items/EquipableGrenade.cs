using GameDevTV.Inventories;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = ("InventorySystem/Equipable Grenade"))]
public class EquipableGrenade : EquipableItem
{
    [SerializeField] private GrenadeProjectile grenadeProjectilePrefab;
    
    public override void Setup(Transform transform)
    {
        GrenadeAction grenadeAction = transform.AddComponent<GrenadeAction>();
        grenadeAction.SetGrenadeProjectilePrefab(grenadeProjectilePrefab);
    }

    public override void RemoveFromUnit(Combat.Unit unit)
    {
        unit.transform.TryGetComponent(out GrenadeAction grenadeAction);
        if (grenadeAction)
        {
            Destroy(grenadeAction);
        }
    }
}
