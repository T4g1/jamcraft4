using UnityEngine;

public class ItemPickup : Interactable 
{
    public Item item;

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