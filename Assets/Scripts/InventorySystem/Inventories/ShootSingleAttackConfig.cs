using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Shoot Single Attack Config"))]
public class ShootSingleAttackConfig : AttackConfig
{
    protected override void Attack()
    {
        Debug.Log("Attacking");
    }
}
