using Combat;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = ("InventorySystem/Equipable Melee Weapon"))]
public class EquipableMeleeWeapon : EquipableWeapon
{
    public override void Setup(Transform transform)
    {
        base.Setup(transform);
        UnitEquipmentVisuals unitEquipmentVisuals = transform.GetComponent<UnitEquipmentVisuals>();
        unitEquipmentVisuals.SpawnWeaponVisual(weaponVisualPrefab);
        
        MeleeAttackAction meleeAttackAction = transform.AddComponent<MeleeAttackAction>();
        meleeAttackAction.SetEquipableMeleeWeapon(this);
    }

    public override void RemoveFromUnit(Combat.Unit unit)
    {
        base.RemoveFromUnit(unit);
        unit.transform.TryGetComponent(out MeleeAttackAction meleeAttackAction);
        if (meleeAttackAction)
        {
            Destroy(meleeAttackAction);
        }
    }
}
