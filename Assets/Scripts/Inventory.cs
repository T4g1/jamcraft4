using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int space = 20;

    public List<Item> items = new List<Item>();

    void Awake()
    {
        if (instance != null) {
            Debug.Log("More than on inventory created!");
        }

        instance = this;
    }

    public bool Add(Item item)
    {
        if (items.Count < space) {
            items.Add(item);

            return true;
        }

        return false;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
