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
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory")) {
            inventory.Toggle();
        }
        
        if (Input.GetButtonDown("Cancel")) {
            escapeMenu.Toggle();
        }

        Cursor.visible = IsOpen();
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
}
