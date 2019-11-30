using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private string textContent = "E";
    public string TextContent {
        set {
            textContent = value;
            OnValidate();
        }
        get { return textContent; }
    }
    
    private Text tooltipText;
    private Vector3 goalPosition;   // Where in the world to display it


    void OnValidate()
    {
        tooltipText = GetComponentInChildren<Text>();
        tooltipText.text = textContent;
    }

    void Start()
    {
        tooltipText = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (!gameObject.activeSelf) {
            return;
        }

        UpdatePosition();
    }

    void UpdatePosition()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        
        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(goalPosition);
        
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform) canvas.transform,
            screenPoint,
            null,
            out canvasPosition
        );
        
        transform.localPosition = canvasPosition;
    }

    public void SetWorldPosition(Vector3 position)
    {
        goalPosition = position;
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
