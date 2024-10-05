using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = ("InventorySystem/Unarmed Melee Weapon"))]
public class UnarmedMeleeWeapon : EquipableWeapon
{
    public override void Setup(Transform transform)
    {
        MeleeAttackAction meleeAttackAction = transform.AddComponent<MeleeAttackAction>();
    }

    public override void RemoveFromUnit(Unit unit)
    {
        unit.transform.TryGetComponent(out MeleeAttackAction meleeAttackAction);
        if (meleeAttackAction)
        {
            Destroy(meleeAttackAction);
        }
    }
}
