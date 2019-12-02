using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : 
    MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image icon = null;
    [SerializeField]
    protected Button itemButton = null;
    [SerializeField]
    private Button removeButton = null;
    [SerializeField]
    private Tooltip tooltip = null;

    private Item item;
    public Item Item {
        get { return item; }
    }
    
    
    void Start()
    {
        Assert.IsNotNull(icon);
        Assert.IsNotNull(tooltip);
        Assert.IsNotNull(itemButton);
        Assert.IsNotNull(removeButton);

        tooltip.Hide();
    }

    public virtual bool AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.sprite;
        icon.enabled = true;
        itemButton.interactable = true;
        removeButton.interactable = true;

        tooltip.TextContent = item.itemName;

        return true;
    }

    public virtual void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        itemButton.interactable = false;
        removeButton.interactable = false;

        tooltip.Hide();
    }

    public void OnRemoveButton()
    {
        Inventory.Instance.Remove(item);
    }

    public void OnUseItem()
    {
        if (IsFree()) {
            return;
        }

        if (item.Use()) {
            Inventory.Instance.Remove(item);
        }
    }

    public bool IsFree()
    {
        return item == null;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        tooltip.Show();
    }

    public void OnPointerExit(PointerEventData data)
    {
        tooltip.Hide();
    }
}
