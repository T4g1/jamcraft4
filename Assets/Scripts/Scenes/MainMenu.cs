using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string themeName;
    [SerializeField]
    public float themIntensity = 50.0f;
    private FMOD.Studio.EventInstance theme;
    [SerializeField]
    private GameObject mouseLight = null;


    void Start()
    {
        Assert.IsNotNull(mouseLight);

        theme = FMODUnity.RuntimeManager.CreateInstance(themeName);
        theme.setParameterByName("intensity", themIntensity);
        theme.start();
    }

    void Update()
    {
        Vector3 mousePosition = Utility.GetMouseWorldPosition();
        mouseLight.transform.position = new Vector3(
            mousePosition.x,
            mousePosition.y,
            0f
        );
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
