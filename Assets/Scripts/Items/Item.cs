using UnityEngine;

public class Item : ScriptableObject
{
    public Sprite sprite;

    /**
     * Return true is object is consumed
     */
    public bool Use()
    {
        // TODO: Place in crafting grid
        return false;
    }
}