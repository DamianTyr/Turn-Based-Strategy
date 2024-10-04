using GameDevTV.Inventories;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = ("InventorySystem/Equipable Gun"))]
public class EquipableGun : EquipableItem
{
   [SerializeField] private LayerMask obstacleLayerMask;
   [SerializeField] private BulletProjectile _bulletProjectile;
   public override void Setup(Transform transform)
   {
      ShootAction shootAction = transform.AddComponent<ShootAction>();
      shootAction.SetBulletProjectile(_bulletProjectile);
      shootAction.SetObstacleLayerMask(obstacleLayerMask);
   }

   public override void RemoveFromUnit(Unit unit)
   {
      unit.transform.TryGetComponent(out ShootAction shootAction);
      if (shootAction)
      {
         Destroy(shootAction);
      }
   }
}
