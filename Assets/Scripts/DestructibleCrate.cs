using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour, IDamageable
{
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;
    
    private GridPosition _gridPosition;

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
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
        
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, Quaternion.identity);
        ApplyExplosionToChildren(crateDestroyedTransform, 300f, explosionPosition, 10f);
        
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
            
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
