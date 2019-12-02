using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour, IPointerExitHandler
{
    private Text textObject = null;
    bool confirmationShown = false;

    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField]
    private Color confirmColor = Color.red;
    [SerializeField]
    private string confirmText = "Confirm ?";
    private string normalText = "";
    [SerializeField] 
    private UnityEvent onButtonConfirmed = null;


    void Start()
    {
        textObject = GetComponentInChildren<Text>();

        Assert.IsNotNull(textObject);
        Assert.IsNotNull(methods);

        normalText = textObject.text;
    }

    public void OnButton()
    {
        if (confirmationShown) {
            ShowNormal();

            onButtonConfirmed.Invoke();
        }
        else {
            ShowConfirm();
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        ShowNormal();
    }

    public void ShowNormal()
    {
        GetComponent<Image>().color = normalColor;
        textObject.text = normalText;

        confirmationShown = false;
    }

    public void ShowConfirm()
    {
        GetComponent<Image>().color = confirmColor;
        textObject.text = confirmText;

        confirmationShown = true;
    }
}
