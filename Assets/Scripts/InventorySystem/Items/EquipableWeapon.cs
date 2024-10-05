
using GameDevTV.Inventories;
using UnityEngine;

public abstract class EquipableWeapon : EquipableItem
{
   [Header("Equipable Weapon Visuals:")]
   [SerializeField] protected GameObject _weaponVisualPrefab;
   
   [SerializeField] protected AnimationClip idleAnimationClip;
   [SerializeField] protected AnimationClip runAnimationClip;
   [SerializeField] protected AnimationClip attackAnimationClip;

   public override void Setup(Transform transform)
   {
      UnitEquipmentVisuals unitEquipmentVisuals = transform.GetComponent<UnitEquipmentVisuals>();
      unitEquipmentVisuals.SpawnWeaponVisual(_weaponVisualPrefab);
   }
   
   public override void RemoveFromUnit(Unit unit)
   {
      UnitEquipmentVisuals unitEquipmentVisuals = unit.transform.GetComponent<UnitEquipmentVisuals>();
      unitEquipmentVisuals.DestroyWeaponVisual();
   }
}
