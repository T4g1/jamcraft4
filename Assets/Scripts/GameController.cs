﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    private FMOD.Studio.EventInstance mainTheme;

    [FMODUnity.EventRef]
    public string mainThemeName;

    [SerializeField]
    private Tween intensityTween = null;

    private int enemyCount = 3;
    private int enemyAggro = 0;
    public int EnemyAggro
    {
        get { return enemyAggro; }
        set {
            enemyAggro = Math.Max(0, Math.Min(value, enemyCount));
            UpdateMainThemeIntensity();
        }
    }


    public static GameController Instance { get; private set; }
 

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        Assert.IsNotNull(intensityTween);

        intensityTween.Interpolate(100.0f, 100.0f);

        mainTheme = FMODUnity.RuntimeManager.CreateInstance(mainThemeName);
        mainTheme.start();
    }

    void Update() 
    {
        mainTheme.setParameterByName(
            "intensity", 
            (float) intensityTween.GetValue()
        );
    }

    void UpdateMainThemeIntensity()
    {
        int intensity = (int)(enemyAggro * 100.0f / enemyCount);

        intensityTween.Interpolate(intensityTween.GetValue(), intensity);
    }
}
