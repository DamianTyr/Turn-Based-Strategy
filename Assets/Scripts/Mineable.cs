using System;
using System.Collections;
using UnityEngine;

public class Mineable : MonoBehaviour
{
    
    public static Action<GridPosition> OnAnyMineableSpawned;
    [SerializeField] private int health = 100;
    [SerializeField] private Transform minebleVisual;
    [SerializeField] private Transform mineableVisualShattered;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRange;
    private GridPosition _gridPosition;

    private void Start()
    {
        _gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        OnAnyMineableSpawned?.Invoke(_gridPosition);
        StartCoroutine(SetupIsWalkable());
        ColonyGrid.Instance.SetMinableAtPosition(_gridPosition, this);
    }

    private IEnumerator SetupIsWalkable()
    {
        yield return null;
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, false);
    }

    public void Mine(int mineAmount, Action onBlockMined)
    {
        health -= mineAmount;
        Debug.Log(health);
        if (health <= 0)
        {
            minebleVisual.gameObject.SetActive(false);
            mineableVisualShattered.gameObject.SetActive(true);
            ApplyExplosionToChildren(this.transform, transform.position);
            Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, true);
            onBlockMined();
        }
    }
    
    private void ApplyExplosionToChildren(Transform root, Vector3 explosionPosition)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            
            ApplyExplosionToChildren(child, explosionPosition);
        }
    }
}
