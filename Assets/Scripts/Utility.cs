using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Utility
{
    public static void AssertArrayNotNull<T>(List<T> array) where T : class
    {
        Assert.IsTrue(array.Count > 0);
        foreach (T content in array) {
            Assert.IsNotNull(content);
        }
    }

    public static GameObject Instantiate(
        GameObject prefab, Vector3 position)
    {
        return GameController.Instance.Instantiate(prefab, position);
    }
}