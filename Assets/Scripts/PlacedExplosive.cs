using System;
using UnityEngine;

public class PlacedExplosive : MonoBehaviour
{
    [SerializeField] private int explosionDamage = 100;
    [SerializeField] private Transform explosionVFXPrefab;
    [SerializeField] private Transform explosionPoint;
    
    public void Explode()
    {
        float damageRadius = 4f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider collider in colliderArray)
        {
            if (collider == transform.GetComponent<Collider>()) continue;
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(explosionDamage, transform);
            }
        }
        
        ScreenShake.Instance.Shake(5f);
        Instantiate(explosionVFXPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);

        Destructible destructible = GetComponent<Destructible>();
        destructible.Damage(25, explosionPoint);
        
        Debug.Log("Boom");
    }
}
