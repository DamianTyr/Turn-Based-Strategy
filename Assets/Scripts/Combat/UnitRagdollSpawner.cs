using UnityEngine;

namespace Combat
{
    public class UnitRagdollSpawner : MonoBehaviour
    {
        [SerializeField] private Transform ragDollPrefab;
        [SerializeField] private Transform originalRootBone;
        private HealthSystem _healthSystem;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.OnDead += HealthSystem_OnDead; 
        }

        private void HealthSystem_OnDead(object sender, Transform damageDealerTransform)
        {
            Transform ragdollTransform = Instantiate(ragDollPrefab, transform.position, transform.rotation);
            UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
            unitRagdoll.Setup(originalRootBone, damageDealerTransform);
        }
    }
}
