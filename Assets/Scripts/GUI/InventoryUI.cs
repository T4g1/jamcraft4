using UnityEngine;
using UnityEngine.Assertions;

public class InventoryUI : Popup
{
    [SerializeField]
    private Transform itemsParent = null;

    private Inventory inventory;
    private InventorySlot[] slots;


    protected override void Start()
    {
        Assert.IsNotNull(itemsParent);

        inventory = Inventory.Instance;
        inventory.OnItemChanged += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        base.Start();
    }

    void OnEnable()
    {
        if (inventory) {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++) {
            if (i < inventory.items.Count) {
                slots[i].AddItem(inventory.items[i]);
            } 
            else {
                slots[i].ClearSlot();
            }
        }
    }
}
