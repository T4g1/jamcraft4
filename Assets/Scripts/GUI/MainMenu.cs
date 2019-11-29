using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
