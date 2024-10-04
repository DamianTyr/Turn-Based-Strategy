using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform shootPointTransform;

    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;
    
    private void Awake()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnOnStopMoving;
        }
        
        if (TryGetComponent(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnOnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnOnSwordActionCompleted;
        }
    }

    private void Start()
    {
        EquipRifle();
    }

    private void SwordAction_OnOnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }
    
    private void SwordAction_OnOnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle(); 
    }

    public void TriggerShoot()
    {
        animator.SetTrigger("Shoot");
    }

    private void MoveAction_OnOnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    public void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    public void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }

    public Transform GetShootPointTransform()
    {
        return shootPointTransform;
    }
}
