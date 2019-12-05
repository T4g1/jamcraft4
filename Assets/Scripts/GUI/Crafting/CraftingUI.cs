using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CraftingUI : Popup
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
    [SerializeField]
    private GameObject onCraftedNotification = null;
    
    [SerializeField] 
    private UnityEvent onCrafted = null;

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

            slot.ClearSlotToInventory();
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

    public void OnCraft()
    {
        // Check the grid is complete
        foreach (CraftingSlot slot in slots) {
            if (slot.IsFree()) {
                return;
            }
        }

        // Check the grid is complete
        foreach (CraftingSlot slot in slots) {
            Utility.GetWeapon().SetPart((WeaponPart) slot.Item);
        }

        ConsumeItems();
        
        GameUIController.Instance.ShowNotification(onCraftedNotification);

        onCrafted.Invoke();
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

    /**
     * Player has access to crafting grid
     */
    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public override void Open()
    {
        base.Open();
        GameUIController.Instance.Inventory.Open();
    }

    public override void Close()
    {
        base.Close();
        GameUIController.Instance.Inventory.Close();
    }
}
