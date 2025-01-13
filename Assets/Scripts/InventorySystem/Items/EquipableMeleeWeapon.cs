using Mission;
using Unity.VisualScripting;
using UnityEngine;
using Transform = UnityEngine.Transform;

[CreateAssetMenu(menuName = ("InventorySystem/Equipable Melee Weapon"))]
public class EquipableMeleeWeapon : EquipableWeapon
{
    public override void Setup(Transform transform)
    {
        base.Setup(transform);
        EquipmentVisuals equipmentVisuals = transform.GetComponent<EquipmentVisuals>();
        equipmentVisuals.SpawnWeaponVisual(weaponVisualPrefab);
        
        MeleeAttackAction meleeAttackAction = transform.AddComponent<MeleeAttackAction>();
        meleeAttackAction.SetEquipableMeleeWeapon(this);
    }

    public override void RemoveFromUnit(EquipmentSetupManager equipmentSetupManager)
    {
        base.RemoveFromUnit(equipmentSetupManager);
        equipmentSetupManager.transform.TryGetComponent(out MeleeAttackAction meleeAttackAction);
        if (meleeAttackAction)
        {
            Destroy(meleeAttackAction);
        }
    }
}
