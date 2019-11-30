using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string mainThemeName;
    private FMOD.Studio.EventInstance mainTheme;


    void Start()
    {
        Screen.SetResolution(640, 360, false);

        mainTheme = FMODUnity.RuntimeManager.CreateInstance(mainThemeName);
        mainTheme.setParameterByName("intensity", 0.5f);
        mainTheme.start();
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnDestroy()
    {
        mainTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
