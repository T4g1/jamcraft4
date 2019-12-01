using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CraftingSlot : InventorySlot
{
    [SerializeField]
    private PartType type = PartType.NONE;
    public PartType Type {
        get { return type; }
    }

    public override bool AddItem(Item newItem)
    {
        if (!IsFree()) {
            return false;
        };

        bool result = base.AddItem(newItem);
        
        itemButton.interactable = false;

        return result;
    }

    public void ClearSlotToInventory()
    {
        Assert.IsFalse(Inventory.Instance.IsFull());
        Inventory.Instance.Add(Item);

        ClearSlot();
    }
}
