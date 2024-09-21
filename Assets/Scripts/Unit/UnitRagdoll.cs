using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originalRootBone, Transform damageDealerTransform)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);
        
        Vector3 damageDirection = (damageDealerTransform.position - transform.position).normalized;
        Vector3 explosionPosition = transform.position + new Vector3(0, 1f, 0);
        explosionPosition += damageDirection;
        
        
        ApplyExplosiionToRagdoll(ragdollRootBone, 700f,explosionPosition, 10f);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                
                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosiionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            
            ApplyExplosiionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
