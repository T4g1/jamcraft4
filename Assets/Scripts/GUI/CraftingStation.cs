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
    private GameObject actionInfo = null;

    private bool isInRange = false;
    
    public override void Interact()
    {
        isInRange = true;

        actionInfo.SetActive(true);
    }
    
    public override void UnInteract()
    {
        isInRange = false;

        GameController.Instance.CloseCraftingUI();
        actionInfo.SetActive(false);
    }

    void Start()
    {
        Assert.IsNotNull(actionInfo);
    }

    void Update()
    {
        if (Input.GetButtonDown("Use") && isInRange) {
            GameController.Instance.ToggleCraftingUI();
        }
    }
}
