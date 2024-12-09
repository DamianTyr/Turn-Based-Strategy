using System;
using System.Collections;
using Colony;
using DG.Tweening;
using EPOOutline;
using PlayerInput;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Outlinable))]
public class Mineable : MonoBehaviour, IRaycastable
{
    [SerializeField] private int health = 100;
    [Header("Visuals")]
    [SerializeField] private GameObject minebleVisual;
    [SerializeField] private GameObject mineableVisualShattered;
    [Header("Shatter Options")]
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRange;
    [Header("Outlines")]
    [SerializeField] private Outlinable mouseOverOutlinable;
    [SerializeField] private Outlinable mouseClickedOutlineble;
    
    private GridPosition _gridPosition;
    private BoxCollider _boxCollider;
    public static Action<GridPosition> OnAnyMined;
    public static Action<GridPosition, Mineable> OnAnySpawned;
    
    private void Start()
    {
        _gridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        _boxCollider = GetComponent<BoxCollider>();

        StartCoroutine(TriggerOnSpawnNextFrame());
        
        mouseOverOutlinable.enabled = false;
        mouseClickedOutlineble.enabled = false;
    }

    private IEnumerator TriggerOnSpawnNextFrame()
    {
        yield return new WaitForEndOfFrame();
        OnAnySpawned?.Invoke(_gridPosition, this);
    }

    public void Mine(int mineAmount, Action onBlockMined)
    {
        health -= mineAmount;
        if (health <= 0)
        {
            minebleVisual.SetActive(false);
            mineableVisualShattered.SetActive(true);
            _boxCollider.enabled = false;
            StartCoroutine(EnableMeshColliders(mineableVisualShattered.transform));
            ApplyExplosionToChildren(mineableVisualShattered.transform, transform.position);
            StartCoroutine(ShrinkAndRemoveDebris());
            onBlockMined();
            OnAnyMined?.Invoke(_gridPosition);
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
    
    private IEnumerator EnableMeshColliders(Transform root)
    {
        yield return new WaitForEndOfFrame();
        
        foreach (Transform child in mineableVisualShattered.transform)
        {
            if (child.TryGetComponent(out MeshCollider meshCollider))
            {
                meshCollider.enabled = true;
            }
            EnableMeshColliders(child);
        }
    }

    private IEnumerator ShrinkAndRemoveDebris()
    {
        yield return new WaitForSeconds(5f);
        foreach (Transform child in mineableVisualShattered.transform)
        {
            child.transform.DOScale(new Vector3(.1f, .1f, .1f), 2f);
        }
    }

    public CursorType GetCursorType()
    {
        return CursorType.Mining;
    }

    public bool HandleRaycast(MouseInputHandler callingController, RaycastHit hit)
    {
        mouseOverOutlinable.enabled = true;
        return true;
    }

    public void HandleRaycastStop()
    {
        mouseOverOutlinable.enabled = false;
    }

    public void HandleMouseClick()
    {
        ColonyTasksManager.Instance.RegisterTask(_gridPosition, ColonyActionType.Mining);
        mouseClickedOutlineble.enabled = true;
    }
}
