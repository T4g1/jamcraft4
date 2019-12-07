using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeySetting : MonoBehaviour
{
    [SerializeField]
    private string action = "Action";
    [SerializeField]
    private KeyType type = KeyType.KeyDown;
    [SerializeField]
    private int mouseButton = 0;
    [SerializeField]
    private KeyCode key = KeyCode.Space;
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
}

public enum KeyType
{
    KeyDown = EventType.KeyDown,
    MouseDown = EventType.MouseDown
}