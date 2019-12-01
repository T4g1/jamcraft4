using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuUI : Popup
{
    private ConfirmButton quitButton = null;


    new void Start()
    {
        quitButton = GetComponentInChildren<ConfirmButton>();
        Debug.Log(quitButton);
        quitButton.OnButtonConfirmed += OnQuitButton;
    }

    void OnDestroy()
    {
        quitButton.OnButtonConfirmed -= OnQuitButton;
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
