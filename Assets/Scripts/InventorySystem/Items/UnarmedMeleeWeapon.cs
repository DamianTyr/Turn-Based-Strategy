using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = ("InventorySystem/Unarmed Melee Weapon"))]
public class UnarmedMeleeWeapon : EquipableWeapon
{
    public override void Setup(Transform transform)
    {
        base.Setup(transform);
        MeleeAttackAction meleeAttackAction = transform.AddComponent<MeleeAttackAction>();
        meleeAttackAction.SetEquipableMeleeWeapon(this);
    }

    public override void RemoveFromUnit(EquipmentSetupHandler equipmentSetupHandler)
    {
        equipmentSetupHandler.transform.TryGetComponent(out MeleeAttackAction meleeAttackAction);
        if (meleeAttackAction)
        {
            Destroy(meleeAttackAction);
        }
    }
}
