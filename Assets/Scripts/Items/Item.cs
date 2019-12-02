using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public bool consumable = false;

    /**
     * Return true is object is consumed
     */
    public virtual bool Use()
    {
        // TODO: Place in crafting grid
        return false;
    }

    public virtual PartType GetPartType()
    {
        return PartType.NONE;
    }

    public virtual void OnPickedUp()
    {
        // Override behaviour
    }
}