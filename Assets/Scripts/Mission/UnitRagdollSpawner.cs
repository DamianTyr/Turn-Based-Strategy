using UnityEngine;

namespace Mission
{
    public class UnitRagdollSpawner : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Transform ragDollPrefab;
        [SerializeField] private UnityEngine.Transform originalRootBone;
        private HealthSystem _healthSystem;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.OnDead += HealthSystem_OnDead; 
        }

        private void HealthSystem_OnDead(object sender, UnityEngine.Transform damageDealerTransform)
        {
            UnityEngine.Transform ragdollTransform = Instantiate(ragDollPrefab, transform.position, transform.rotation);
            UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
            unitRagdoll.Setup(originalRootBone, damageDealerTransform);
        }
    }
}
