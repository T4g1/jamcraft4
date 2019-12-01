using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * DO NOT PUT CLOSE TO TELEPORTER AS TELEPORTATION BREAKS THE COLLISION
 * DETECTION SO THIS IS NEVER DE-ACTIVATED
 */
public class CraftingStation : Interactable
{
    [SerializeField]
    private Tooltip actionInfo = null;

    private bool isInRange = false;
    
    public override void Interact()
    {
        isInRange = true;

        actionInfo.Show();
    }
    
    public override void UnInteract()
    {
        isInRange = false;

        GameUIController.Instance.CloseCrafting();
        actionInfo.Hide();
    }

    void Start()
    {
        Assert.IsNotNull(actionInfo);
    }

    void Update()
    {
        if (Input.GetButtonDown("Use") && isInRange) {
            GameUIController.Instance.ToggleCrafting();
        }
    }
}
