using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CraftingUI : MonoBehaviour
{
    #region Singleton
    public static CraftingUI Instance { get; private set; }

    void InitInstance()
    {
        if (Instance != null) {
            Debug.Log("More than on crafting UI created!");
        }

        Instance = this;
    }
    #endregion

    [SerializeField]
    private GameObject craftingGrid = null;

    private Inventory inventory;
    private CraftingSlot[] slots;

    
    void Awake()
    {
        Assert.IsNotNull(craftingGrid);
        
        InitInstance();

        inventory = Inventory.Instance;

        slots = craftingGrid.GetComponentsInChildren<CraftingSlot>();
    }

    void OnDisable()
    {
        CancelCrafting();
    }

    /**
     * Removes items and place them back in inventory
     */
    void CancelCrafting()
    {
        foreach (CraftingSlot slot in slots) {
            if (slot.IsFree()) {
                continue;
            }

            Assert.IsFalse(inventory.IsFull());

            inventory.Add(slot.Item);
            slot.ClearSlot();
        }
    }

    void ConsumeItems()
    {
        foreach (CraftingSlot slot in slots) {
            if (slot.IsFree()) {
                continue;
            }
            
            slot.ClearSlot();
        }
    }

    void OnCraft()
    {
        // TODO
        //ConsumeItems();
    }

    /**
     * Add an item into the crafting grid
     */
    public bool AddItem(Item item)
    {
        PartType type = item.GetPartType();
        foreach (CraftingSlot slot in slots) {
            if (slot.Type == type) {
                return slot.AddItem(item);
            }
        }

        return false;
    }
}
