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

    public static Vector3 GetMouseInScreenPosition()
    {
        float marginTop = Screen.height / 3;
        float marginBottom = Screen.height / 3;
        float marginLeft = Screen.width / 3;
        float marginRight = Screen.width / 3;

        // Confines the mouse in the screen coordinates
        Vector3 screenPosition = Input.mousePosition;
        if (screenPosition.x < marginRight) {
            screenPosition.x = marginRight;
        }
        if (screenPosition.y < marginTop) {
            screenPosition.y = marginTop;
        }
        if (screenPosition.x > Screen.width - marginLeft) {
            screenPosition.x = Screen.width - marginLeft;
        }
        if (screenPosition.y > Screen.height - marginBottom) {
            screenPosition.y = Screen.height - marginBottom;
        }

        return screenPosition;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPosition = Utility.GetMouseInScreenPosition();
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}