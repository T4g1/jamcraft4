using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
    }

    public void SettingSelected(KeySetting setting)
    {
        foreach(KeySetting keySetting in keySettings) {
            keySetting.OnDeselect();
        }

        setting.OnSelect();
    }
}
