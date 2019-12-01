using UnityEngine;

[ExecuteInEditMode]
public class ItemPickup : Interactable 
{
    [SerializeField]
    private Item item;
    public Item Item {
        get { return item; }
        set 
        { 
            item = value;
            GetComponent<SpriteRenderer>().sprite = item.sprite;
        }
    }

    void OnValidate()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer && item) {
            renderer.sprite = item.sprite;
        }
    }

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {
        if (item.consumable || Inventory.Instance.Add(item)) {
            item.OnPickedUp();
            
            Destroy(gameObject);
        }
    }
}