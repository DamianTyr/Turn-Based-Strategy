using System;
using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable
{
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform destroyedPrefab;
    
    private GridPosition _gridPosition;

    private void Start()
    {
        _gridPosition = MissionGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public void Damage(int damageAmount, Transform damageDealerTransform)
    {
        Vector3 damageDirection = (damageDealerTransform.position - transform.position).normalized;
        Vector3 explosionPosition = transform.position + new Vector3(0, 1f, 0);
        explosionPosition += damageDirection;
        
        Transform destroyedTransform = Instantiate(destroyedPrefab, transform.position, Quaternion.identity);
        ApplyExplosionToChildren(destroyedTransform, 300f, explosionPosition, 10f);
        
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
    
    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            
            ApplyExplosionToChildren(child,  explosionForce, explosionPosition,  explosionRange);
        }
    }
}
