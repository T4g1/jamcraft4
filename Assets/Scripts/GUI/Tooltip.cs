using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject follow = null;
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private bool defaultActive = false;
    [SerializeField]
    private float textMargin = 2.0f;
    [SerializeField]
    private string textContent = "E";
    public string TextContent {
        set {
            textContent = value;
            UpdateSize();
        }
        get { return textContent; }
    }
    
    private Text tooltipText;


    void UpdateSize()
    {
        tooltipText = GetComponentInChildren<Text>();
        tooltipText.text = textContent;

        RectTransform rectTransform = GetComponent<RectTransform>();
        RectTransform tooltipTransform = 
            tooltipText.GetComponent<RectTransform>();
        
        rectTransform.sizeDelta = tooltipTransform.sizeDelta; 
        rectTransform.sizeDelta += new Vector2(textMargin, textMargin) * 2;
    }

    void Start()
    {
        tooltipText = GetComponentInChildren<Text>();

        if (defaultActive) {
            Show();
        }
        else {
            Hide();
        }
    }

    void Update()
    {
        if (!gameObject.activeSelf) {
            return;
        }

        UpdateSize();
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (follow == null) {
            return;
        }
        
        Canvas canvas = GetComponentInParent<Canvas>();
        
        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(
            follow.transform.position + offset
        );
        
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
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
