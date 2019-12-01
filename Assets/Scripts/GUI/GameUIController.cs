using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
        if (Input.GetButtonDown("Inventory") && !escapeMenu.IsOpen()) {
            inventory.Toggle();
        }
        
        if (Input.GetButtonDown("Cancel")) {
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
}
