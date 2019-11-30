using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string themeName;
    [SerializeField]
    public float themIntensity = 50.0f;
    private FMOD.Studio.EventInstance theme;


    void Start()
    {
        //Screen.SetResolution(640, 360, false);

        theme = FMODUnity.RuntimeManager.CreateInstance(themeName);
        theme.setParameterByName("intensity", themIntensity);
        theme.start();
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
        theme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
