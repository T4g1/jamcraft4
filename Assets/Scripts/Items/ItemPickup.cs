using UnityEngine;
using UnityEngine.Assertions;

[ExecuteInEditMode]
public class ItemPickup : Interactable 
{
    [FMODUnity.EventRef]
    public string pickupSFX = "";

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

    void Start()
    {
        Assert.IsTrue(pickupSFX != "");
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
            Utility.PlaySFX(pickupSFX);

            item.OnPickedUp();
            
            Destroy(gameObject);
        }
    }
}