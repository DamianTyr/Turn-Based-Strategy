using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = ("InventorySystem/Equipable Melee Weapon"))]
public class EquipableMeleeWeapon : EquipableWeapon
{
    public override void Setup(Transform transform)
    {
        base.Setup(transform);
        MeleeAttackAction meleeAttackAction = transform.AddComponent<MeleeAttackAction>();
    }

    public override void RemoveFromUnit(Unit unit)
    {
        base.RemoveFromUnit(unit);
        unit.transform.TryGetComponent(out MeleeAttackAction meleeAttackAction);
        if (meleeAttackAction)
        {
            Destroy(meleeAttackAction);
        }
    }
}
