using System;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFXPrefab;
    private Vector3 _targetPosition;
    
    public void Setup(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);
        
        float moveSpeed = 200f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = _targetPosition;
            trailRenderer.transform.SetParent(null);

            Instantiate(bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
}
