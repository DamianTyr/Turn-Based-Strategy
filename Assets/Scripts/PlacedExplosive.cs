using System;
using UnityEngine;

public class PlacedExplosive : MonoBehaviour
{
    [SerializeField] private int explosionDamage = 100;
    [SerializeField] private Transform explosionVFXPrefab;

    public static event EventHandler onAnyPlacedExplosiveDetonation;
    
    public void Explode()
    {
        float damageRadius = 4f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, damageRadius);
        Debug.Log(transform.position);

        foreach (Collider collider in colliderArray)
        {
            Debug.Log(collider.transform.name);
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log("Found Some damageable to damage");
                damageable.Damage(explosionDamage, transform);
            }
        }
        
        onAnyPlacedExplosiveDetonation?.Invoke(this, EventArgs.Empty);
        Instantiate(explosionVFXPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
        //Destroy(gameObject);
        
        Debug.Log("Boom");
    }
}
