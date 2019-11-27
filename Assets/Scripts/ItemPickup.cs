using UnityEngine;

public class ItemPickup : Interactable 
{
    private Item item;
    public Item Item {
        get { return item; }
        set 
        { 
            item = value;
            GetComponent<SpriteRenderer>().sprite = item.sprite;
        }
    }

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {
        if (Inventory.Instance.Add(item)) {
            Destroy(gameObject);
        }
    }
}