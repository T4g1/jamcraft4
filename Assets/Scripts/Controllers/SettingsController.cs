using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

/**
 * Hacked away a semi-real Input manager as Unity is pure garbage
 * Stores a tuple for each action with type and value mapped to it
 */
public class SettingsController : MonoBehaviour
{
    #region Singleton
    public static SettingsController Instance { get; private set; }

    void InitInstance()
    {
        if (Instance != null) {
            Debug.Log("More than one SettingsController created!");
        }

        Instance = this;
    }

    private void Awake() 
    {
        InitInstance();
    }
    #endregion

    [SerializeField]
    private GameObject keySettingContainer = null;
    private KeySetting[] keySettings;


    void Start()
    {
        Assert.IsNotNull(keySettingContainer);

        keySettings = 
            keySettingContainer.GetComponentsInChildren<KeySetting>(true);

        foreach (KeySetting keySetting in keySettings) {
            keySetting.Load();
            keySetting.Save();
        }
    }

    public void SettingSelected(KeySetting setting)
    {
        foreach (KeySetting keySetting in keySettings) {
            keySetting.OnDeselect();
        }

        setting.OnSelect();
    }

    public void OnReturn()
    {
        SceneManager.UnloadSceneAsync("Settings");
    }
}
