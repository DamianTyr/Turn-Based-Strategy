
using GameDevTV.Inventories;
using UnityEngine;

public abstract class EquipableWeapon : EquipableItem
{
   [Header("Equipable Weapon Visual Prefab:")]
   [SerializeField] protected GameObject _weaponVisualPrefab;
   [Header("Animation Clips:")]
   [SerializeField] protected AnimationClip idleAnimationClip;
   [SerializeField] protected AnimationClip runAnimationClip;
   [SerializeField] protected AnimationClip attackAnimationClip;

   public override void Setup(Transform transform)
   {
      MoveAction moveAction = transform.GetComponent<MoveAction>();
      moveAction.SetAnimationClips(idleAnimationClip, runAnimationClip);
   }
   
   public override void RemoveFromUnit(Unit unit)
   {
      UnitEquipmentVisuals unitEquipmentVisuals = unit.transform.GetComponent<UnitEquipmentVisuals>();
      unitEquipmentVisuals.DestroyWeaponVisual();
   }
}
