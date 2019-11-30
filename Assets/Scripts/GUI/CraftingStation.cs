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
    [SerializeField]
    private float actionInfoOffset = 0.5f;

    private bool isInRange = false;
    
    public override void Interact()
    {
        isInRange = true;

        actionInfo.Show();
    }
    
    public override void UnInteract()
    {
        isInRange = false;

        GameController.Instance.CloseCraftingUI();
        actionInfo.Hide();
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

        actionInfo.SetWorldPosition(new Vector3(
            transform.position.x,
            transform.position.y + actionInfoOffset,
            transform.position.z
        ));
    }
}
