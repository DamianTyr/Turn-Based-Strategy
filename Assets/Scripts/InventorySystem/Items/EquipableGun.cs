using GameDevTV.Inventories;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = ("InventorySystem/Equipable Gun"))]
public class EquipableGun : EquipableWeapon
{
   [Header("Equipable Gun settings:")]  
   [SerializeField] private LayerMask obstacleLayerMask;
   [SerializeField] private BulletProjectile _bulletProjectile;
   public override void Setup(Transform transform)
   {
      UnitEquipmentVisuals unitEquipmentVisuals = transform.GetComponent<UnitEquipmentVisuals>();
      GameObject spawnedWeaponVisual = unitEquipmentVisuals.SpawnWeaponVisual(_weaponVisualPrefab);
      WeaponShootPoint weaponShootPoint = spawnedWeaponVisual.GetComponentInChildren<WeaponShootPoint>();
      
      ShootAction shootAction = transform.AddComponent<ShootAction>();
      shootAction.SetBulletProjectile(_bulletProjectile);
      shootAction.SetObstacleLayerMask(obstacleLayerMask);
      shootAction.SetShootPointTransform(weaponShootPoint.transform);
   }

   public override void RemoveFromUnit(Unit unit)
   {
      base.RemoveFromUnit(unit);
      unit.transform.TryGetComponent(out ShootAction shootAction);
      if (shootAction)
      {
         Destroy(shootAction);
      }
   }
}
