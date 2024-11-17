using Mission;
using Unity.VisualScripting;
using UnityEngine;
using Transform = UnityEngine.Transform;

[CreateAssetMenu(menuName = ("InventorySystem/Equipable Gun"))]
public class EquipableGun : EquipableWeapon
{
   [Header("Equipable Gun settings:")]  
   [SerializeField] private LayerMask obstacleLayerMask;
   [SerializeField] private BulletProjectile bulletProjectile;
   public override void Setup(Transform transform)
   {
      base.Setup(transform);
      UnitEquipmentVisuals unitEquipmentVisuals = transform.GetComponent<UnitEquipmentVisuals>();
      GameObject spawnedWeaponVisual = unitEquipmentVisuals.SpawnWeaponVisual(weaponVisualPrefab);
      WeaponShootPoint weaponShootPoint = spawnedWeaponVisual.GetComponentInChildren<WeaponShootPoint>();

      ShootAction shootAction = transform.AddComponent<ShootAction>();
      shootAction.SetEquipableGun(this);
      shootAction.SetShootPointTransform(weaponShootPoint.transform);
   }

   public override void RemoveFromUnit(Mission.Unit unit)
   {
      base.RemoveFromUnit(unit);
      unit.transform.TryGetComponent(out ShootAction shootAction);
      if (shootAction)
      {
         Destroy(shootAction);
      }
   }

   public BulletProjectile GetBulletProjectile()
   {
      return bulletProjectile;
   }

   public LayerMask GetObstaclesLayerMask()
   {
      return obstacleLayerMask;
   }
}
