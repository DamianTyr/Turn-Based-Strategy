using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler<Transform> OnDead;
    public event EventHandler OnDamage;
    
    [SerializeField] private int health = 100;
    private int _healthMax;

    private void Awake()
    {
        _healthMax = health;
    }

    public void Damage(int damageAmount, Transform damageDealerTransform)
    {
        health -= damageAmount;
        OnDamage?.Invoke(this,EventArgs.Empty);
        if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            Die(damageDealerTransform);
        }
    }

    private void Die(Transform damageDealerTransform)
    {
        OnDead?.Invoke(this, damageDealerTransform);
    }

    public float GetHealthNormalized()
    {
        return (float)health / _healthMax;
    }
}
