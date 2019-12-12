using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuUI : Popup
{
    public void OnMenuButton()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void OnSettingsButton()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void OnResumeButton()
    {
        GameUIController.Instance.CloseUI();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public override void Open()
    {
        base.Open();
        Time.timeScale = 0;
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1;
    }
}
