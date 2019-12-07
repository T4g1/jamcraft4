using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeySetting : MonoBehaviour
{
    public string action = "Action";
    public KeyType type = KeyType.KeyDown;
    public int mouseButton = 0;
    public KeyCode key = KeyCode.Space;

    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField]
    private Color selectedColor = Color.blue;

    private string[] mouseButtons = new string[] {
        "Left click",
        "Right click",
        "Middle click"
    };

    private bool isSelected = false;
    private Image image;
    private Text actionText;
    private Text buttonText;
    private Button button;


    void Awake()
    {
        image = GetComponentInChildren<Image>();
        actionText = GetComponentInChildren<Text>();
        button = GetComponentInChildren<Button>();
        buttonText = button.GetComponentInChildren<Text>();

        actionText.text = action;

        OnDeselect();
    }

    public void UpdateButtonText()
    {
        image = GetComponentInChildren<Image>();
        actionText = GetComponentInChildren<Text>();
        button = GetComponentInChildren<Button>();
        buttonText = button.GetComponentInChildren<Text>();

        if (type == KeyType.KeyDown) {
            buttonText.text = key.ToString();
        }
        else {
            if (mouseButton < mouseButtons.Length) {
                buttonText.text = mouseButtons[mouseButton];
            }
            else {
                buttonText.text = "Mouse button " + mouseButton.ToString();
            }
        }
    }

    void OnGUI()
    {
        if (!isSelected) {
            return;
        }

        Event currentEvent = Event.current;
        if (currentEvent.type == EventType.KeyDown) {
            type = KeyType.KeyDown;
            key = currentEvent.keyCode;
        }
        else if (currentEvent.type == EventType.MouseDown) {
            type = KeyType.MouseDown;
            mouseButton = currentEvent.button;
        }
        else {
            return;
        }

        OnDeselect();

        Save();
    }

    public void OnClick()
    {
        if (isSelected) {
            return;
        }

        SettingsController.Instance.SettingSelected(this);
    }

    public void OnDeselect()
    {
        if (image == null) {
            return;
        }

        image.color = normalColor;
        UpdateButtonText();
        isSelected = false;
    }

    public void OnSelect()
    {
        image.color = selectedColor;
        buttonText.text = "Press a key";
        isSelected = true;
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
                key.ToString()
            );
        }
        else {
            PlayerPrefs.SetString(
                action + "_value", 
                mouseButton.ToString()
            );
        }
        
    }

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
            key = (KeyCode)System.Enum.Parse(typeof(KeyCode), value);
        }
        else {
            mouseButton = Int32.Parse(value);
        }

        UpdateButtonText();
    }
}

public enum KeyType
{
    KeyDown = EventType.KeyDown,
    MouseDown = EventType.MouseDown
}