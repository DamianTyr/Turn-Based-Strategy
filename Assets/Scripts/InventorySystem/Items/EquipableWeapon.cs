
using GameDevTV.Inventories;
using Combat;
using UnityEngine;

public abstract class EquipableWeapon : EquipableItem
{
   [Header("Weapon Stats:")] 
   [SerializeField] protected int damage;
   [Header("Equipable Weapon Visual Prefab:")]
   [SerializeField] protected GameObject weaponVisualPrefab;
   [Header("Animation Clips:")]
   [SerializeField] protected AnimationClip idleAnimationClip;
   [SerializeField] protected AnimationClip runAnimationClip;
   [SerializeField] protected AnimationClip attackAnimationClip;
   [Header("Attack Animation Blend Time")]
   [SerializeField] protected float attackAnimationFadeTime;

   public override void Setup(Transform transform)
   {
      MoveAction moveAction = transform.GetComponent<MoveAction>();
      moveAction.SetAnimationClips(idleAnimationClip, runAnimationClip);
   }
   
   public override void RemoveFromUnit(Combat.Unit unit)
   {
      UnitEquipmentVisuals unitEquipmentVisuals = unit.transform.GetComponent<UnitEquipmentVisuals>();
      unitEquipmentVisuals.DestroyWeaponVisual();
   }

   public AnimationClip GetAttackAnimationClip()
   {
      return attackAnimationClip;
   }

   public float GetAttackAnimationFadeTime()
   {
      return attackAnimationFadeTime;
   }

   public int GetDamage()
   {
      return damage;
   }
}
