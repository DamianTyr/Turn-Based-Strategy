using System;
using Grid;
using UnityEngine;

namespace Mission
{
   public class GrenadeProjectile : MonoBehaviour
   {
      public static event EventHandler OnAnyGrenadeExplosion;

      [SerializeField] private Transform grenadeExplosionVFXPrefab;
      [SerializeField] private TrailRenderer trailRenderer;
      [SerializeField] private AnimationCurve arcYAnimationCurve;
   
      private Vector3 _targetPosition;
      private Action _onGrenadeBehaviorComplete;

      private float _totalDistance;
      private Vector3 _positionXZ;
   
      private void Update()
      {
         Vector3 moveDir = (_targetPosition - _positionXZ).normalized;
         float moveSpeed = 15f;
         _positionXZ += moveDir * moveSpeed * Time.deltaTime;

         float distance = Vector3.Distance(_positionXZ, _targetPosition);
         float distanceNormalized = 1 - distance / _totalDistance;

         float maxHeight = _totalDistance / 4f;
         float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
         transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);
       
         float reachedTargetDistance = 0.2f;
         if (Vector3.Distance(transform.position, _targetPosition) < reachedTargetDistance)
         {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
               if (collider.TryGetComponent<IDamageable>(out IDamageable damageable))
               {
                  damageable.TakeDamage(30, this.transform);
               }
            }

            OnAnyGrenadeExplosion?.Invoke(this, EventArgs.Empty);
            ScreenShake.Instance.Shake(5f);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplosionVFXPrefab, _targetPosition + Vector3.up * 1f, Quaternion.identity);
            _onGrenadeBehaviorComplete();
            Destroy(gameObject);
         }
      }
   
      public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
      {
         _onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
         _targetPosition = MissionGrid.Instance.GetWorldPosition(targetGridPosition);

         _positionXZ = transform.position;
         _positionXZ.y = 0;
         _totalDistance = Vector3.Distance(transform.position, _targetPosition);
      }
   }
}
