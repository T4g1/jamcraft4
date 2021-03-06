﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance { get; private set; }

    void InitInstance()
    {
        if (Instance != null) {
            Debug.Log("More than on inventory created!");
        }

        Instance = this;
    }
    #endregion

    public event System.Action OnItemChanged;

    public int space = 20;

    public List<Item> items = new List<Item>();

    void Awake()
    {
        InitInstance();
    }

    public bool Add(Item item)
    {
        if (IsFull()) {
            return false;
        }

        items.Add(item);

        if (OnItemChanged != null) {
            OnItemChanged();
        }

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (OnItemChanged != null) {
            OnItemChanged();
        }
    }

    public bool IsFull()
    {
        return items.Count >= space;
    }
}
