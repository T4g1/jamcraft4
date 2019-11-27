using UnityEngine;

public class Item : ScriptableObject
{
    public Sprite sprite;

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
}