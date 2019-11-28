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
    [SerializeField]
    private float actionInfoOffset = 0.5f;

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

        if (actionInfo.activeSelf) {
            UpdateActionInfoPosition();
        }
    }

    void UpdateActionInfoPosition()
    {
        Canvas canvas = actionInfo.GetComponentInParent<Canvas>();
        Vector3 offsetPosition = new Vector3(
            transform.position.x,
            transform.position.y + actionInfoOffset,
            transform.position.z
        );
        
        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 canvasPosition;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPosition);
        
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform) canvas.transform,
            screenPoint,
            null,
            out canvasPosition
        );
        
        actionInfo.transform.localPosition = canvasPosition;
    }
}
