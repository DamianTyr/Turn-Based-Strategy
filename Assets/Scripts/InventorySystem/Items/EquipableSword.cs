using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = ("InventorySystem/Equipable Sword"))]
public class EquipableSword : EquipableItem
{
    public override void Setup(Transform transform)
    {
        SwordAction swordAction = transform.AddComponent<SwordAction>();
    }

    public override void RemoveFromUnit(Unit unit)
    {
        unit.transform.TryGetComponent(out SwordAction swordAction);
        if (swordAction)
        {
            Destroy(swordAction);
        }
    }
}
