using UnityEngine;

namespace Mission
{
    public interface IDamageable
    {
        public void TakeDamage(int damageAmount, Transform damageDealerTransform);
    }
}
