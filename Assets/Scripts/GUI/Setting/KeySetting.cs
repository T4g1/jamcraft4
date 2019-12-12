using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeySetting : MonoBehaviour
{
    [SerializeField]
    private Action action = null;
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
        Assert.IsNotNull(action);

        image = GetComponentInChildren<Image>();
        actionText = GetComponentInChildren<Text>();
        button = GetComponentInChildren<Button>();
        buttonText = button.GetComponentInChildren<Text>();

        actionText.text = action.description;

        OnDeselect();
    }

    public void UpdateButtonText()
    {
        image = GetComponentInChildren<Image>();
        actionText = GetComponentInChildren<Text>();
        button = GetComponentInChildren<Button>();
        buttonText = button.GetComponentInChildren<Text>();

        if (action.type == KeyType.KeyDown) {
            buttonText.text = action.keyCode.ToString();
        }
        else {
            if (action.mouseButton < mouseButtons.Length) {
                buttonText.text = mouseButtons[action.mouseButton];
            }
            else {
                buttonText.text = 
                    "Mouse button " + action.mouseButton.ToString();
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
            action.type = KeyType.KeyDown;
            action.keyCode = currentEvent.keyCode;
        }
        else if (currentEvent.type == EventType.MouseDown) {
            action.type = KeyType.MouseDown;
            action.mouseButton = currentEvent.button;
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
        action.Save();
    }

    public void Load()
    {
        action.Load();

        UpdateButtonText();
    }
}