﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * No UI can be opened/closed while pause menu is open
 */
public class GameUIController : MonoBehaviour
{
    #region Singleton
    public static GameUIController Instance { get; private set; }

    void InitInstance()
    {
        if (Instance != null) {
            Debug.Log("More than one GameUIController created!");
        }

        Instance = this;
    }
    #endregion

    [SerializeField]
    private Popup escapeMenu = null;
    public Popup EscapeMenu {
        get { return escapeMenu; }
        set {}
    }
    [SerializeField]
    private Popup inventory = null;
    public Popup Inventory {
        get { return inventory; }
        set {}
    }
    [SerializeField]
    private Popup crafting = null;
    public Popup Crafting {
        get { return crafting; }
        set {}
    }

    [SerializeField]
    private GameObject loadingText = null;
    public GameObject LoadingText {
        get { return loadingText; }
        set {}
    }


    private void Awake() {
        InitInstance();
    }

    void Start()
    {
        Assert.IsNotNull(escapeMenu);
        Assert.IsNotNull(inventory);
        Assert.IsNotNull(crafting);

        escapeMenu.OnOpen += OnUIOpen;
        inventory.OnOpen += OnUIOpen;
        crafting.OnOpen += OnUIOpen;
    }

    void Update()
    {
        if (InputController.Instance.GetButtonDown("inventory")) {
            ToggleInventory();
        }
        
        if (InputController.Instance.GetButtonDown("pause")) {
            // Exit any UI open
            if (IsOpen()) {
                CloseUI();
            } 
            // Shows escape menu
            else {
                escapeMenu.Open();
            }
        }

        Cursor.visible = IsOpen();
    }

    void OnDestroy() 
    {
        escapeMenu.OnOpen -= OnUIOpen;
        inventory.OnOpen -= OnUIOpen;
        crafting.OnOpen -= OnUIOpen;
    }

    public bool IsOpen()
    {
        return 
            escapeMenu.IsOpen() || 
            crafting.IsOpen() || 
            inventory.IsOpen();
    }

    public void CloseUI()
    {
        escapeMenu.Close();
        crafting.Close();
        inventory.Close();
    }

    void OnUIOpen()
    {
        // Stop player once when UI is opened
        Utility.GetPlayer().StopMovement();
    }

    public void CloseCrafting()
    {
        if (escapeMenu.IsOpen()) {
            return;
        }

        crafting.Close();
    }

    public void ToggleCrafting()
    {
        if (escapeMenu.IsOpen()) {
            return;
        }

        crafting.Toggle();
    }

    public void ToggleInventory()
    {
        if (escapeMenu.IsOpen()) {
            return;
        }

        // Opening inventory while in crafting will show only inventory
        if (crafting.IsOpen() && inventory.IsOpen()) {
            crafting.Close();
        }
        
        inventory.Toggle();
    }

    public void ShowNotification(GameObject notification)
    {
        GameObject clone = Instantiate(notification);
        clone.transform.SetParent(notification.transform.parent);
        clone.GetComponent<Notification>().Show();
    }
}
