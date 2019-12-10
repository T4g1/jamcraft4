using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InputController : MonoBehaviour
{
    #region Singleton
    public static InputController Instance { get; private set; }

    void InitInstance()
    {
        if (Instance != null) {
            Debug.Log("More than one InputController created!");
        }

        Instance = this;
    }

    private void Awake() 
    {
        InitInstance();
        LoadInputs();
    }
    #endregion

    [SerializeField]
    private List<Action> actions = new List<Action>();


    void LoadInputs()
    {
        foreach (Action action in actions) {
            action.Load();
        }
    }

    public Action GetAction(string actionString)
    {
        foreach (Action action in actions) {
            if (action.action == actionString) {
                return action;
            }
        }

        // Cannot reach this point
        Assert.IsTrue(false);
        return null;
    }

    public int GetHorizontalAxis()
    {
        bool left = GetButton("left");
        bool right = GetButton("right");

        if (left && right) {
            return 0;
        }
        else if (left) {
            return -1;
        }
        else if (right) {
            return 1;
        }
        
        return 0;
    }

    public int GetVerticalAxis()
    {
        bool up = GetButton("up");
        bool down = GetButton("down");

        if (up && down) {
            return 0;
        }
        else if (down) {
            return -1;
        }
        else if (up) {
            return 1;
        }
        
        return 0;
    }

    public bool GetButton(string action)
    {
        return GetAction(action).IsDown();
    }

    public bool GetButtonDown(string action)
    {
        return GetAction(action).IsJustDown();
    }
}