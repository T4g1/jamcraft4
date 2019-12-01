using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour
{
    public event System.Action OnButtonConfirmed;

    bool confirmationShown = false;
    private Text textObject = null;
    private string originalValue = "";

    [SerializeField]
    private string confirmText = "Confirm ?";


    void Start()
    {
        textObject = GetComponentInChildren<Text>();

        Assert.IsNotNull(textObject);

        originalValue = textObject.text;
    }

    public void OnButton()
    {
        if (confirmationShown) {
            textObject.text = originalValue;

            if (OnButtonConfirmed != null) {
                OnButtonConfirmed();
            }
        }
        else {
            textObject.text = confirmText;
        }

        confirmationShown = !confirmationShown;
    }
}
