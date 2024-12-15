using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Core.UI.Dragging;
using InventorySystem.Inventories;

namespace InventorySystem.UI.Inventories
{
    /// <summary>
    /// To be placed on icons representing the item in a slot. Allows the item
    /// to be dragged into other slots.
    /// </summary>
    public class InventoryDragItem : DragItem<InventoryItem>
    {
    }
}