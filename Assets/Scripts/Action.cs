using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New input", menuName = "Input/New input")]
public class Action: ScriptableObject
{
    public string action;
    public string description;
    public KeyType type;
    public KeyCode keyCode;
    public int mouseButton;

    public void Load()
    {
        if (!PlayerPrefs.HasKey(action)) {
            return;
        }

        type = (KeyType)System.Enum.Parse(
            typeof(KeyType), PlayerPrefs.GetString(action) 
        );
        
        string value = PlayerPrefs.GetString(action + "_value");
        if (type == KeyType.KeyDown) {
            keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), value);
        }
        else {
            mouseButton = Int32.Parse(value);
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(
            action, 
            type.ToString()
        );

        if (type == KeyType.KeyDown) {
            PlayerPrefs.SetString(
                action + "_value", 
                keyCode.ToString()
            );
        }
        else {
            PlayerPrefs.SetString(
                action + "_value", 
                mouseButton.ToString()
            );
        }
    }

    public bool IsDown()
    {
        if (type == KeyType.KeyDown) {
            return Input.GetKey(keyCode);
        }
        else {
            return Input.GetMouseButton(mouseButton);
        }
    }

    public bool IsJustDown()
    {
        if (type == KeyType.KeyDown) {
            return Input.GetKeyDown(keyCode);
        }
        else {
            return Input.GetMouseButtonDown(mouseButton);
        }
    }
}

public enum KeyType
{
    KeyDown = EventType.KeyDown,
    MouseDown = EventType.MouseDown
}
