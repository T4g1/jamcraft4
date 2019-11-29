using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Utility
{
    public static T RandomElement<T>(T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

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

    public static Vector3 GetMouseInScreenPosition()
    {
        // Confines the mouse in the screen coordinates
        Vector3 screenPosition = Input.mousePosition;
        if (screenPosition.x < 0) {
            screenPosition.x = 0;
        }
        if (screenPosition.y < 0) {
            screenPosition.y = 0;
        }
        if (screenPosition.x > Screen.width) {
            screenPosition.x = Screen.width;
        }
        if (screenPosition.y > Screen.height) {
            screenPosition.y = Screen.height;
        }

        screenPosition.z = 0.0f;

        return screenPosition;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPosition = Utility.GetMouseInScreenPosition();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0.0f;

        return worldPosition;
    }

    public static TileBase GetWall()
    {
        return GameController.Instance.Wall;
    }

    public static TileBase GetFloor()
    {
        return GameController.Instance.Floor;
    }
}