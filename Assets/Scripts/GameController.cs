using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController Instance { get; private set; }

    void InitInstance()
    {
        if (Instance != null) {
            Debug.Log("More than on inventory created!");
        }

        Instance = this;
    }
    #endregion
    
    private FMOD.Studio.EventInstance mainTheme;

    public const uint TILE_SIZE = 16;

    [FMODUnity.EventRef]
    public string mainThemeName;

    [SerializeField]
    private Tween intensityTween = null;
    
    [SerializeField]
    private GameObject pickUpPrefab = null;
    
    public GameObject dynamicHolder = null;

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


    private void Awake() {
        InitInstance();
    }

    void Start()
    {
        Assert.IsNotNull(intensityTween);
        Assert.IsNotNull(pickUpPrefab);

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

    /**
     * Generate a random weapon part that can be picked up
     */
    public void CreatePickUp(Vector3 where)
    {
        GameObject pickUpObject = Instantiate(pickUpPrefab);
        pickUpObject.transform.parent = dynamicHolder.transform;
        pickUpObject.transform.position = where;

        ItemPickup pickUp = pickUpObject.GetComponent<ItemPickup>();
        pickUp.item = GenerateRandomWeaponPart(PartType.BARREL);
    }

    public WeaponPart GenerateRandomWeaponPart(
        PartType forcePart = PartType.NONE
    )
    {
        WeaponPart part = ScriptableObject.CreateInstance<WeaponPart>();

        PartType partType = forcePart;
        if (partType == PartType.NONE) {
            partType = (PartType) UnityEngine.Random.Range(
                1, 
                Enum.GetValues(typeof(PartType)).Cast<int>().Max()
            );
        }

        switch (partType) {
            case PartType.QUIVER:
                part.RandomizeQuiver();
                break;
                
            case PartType.BARREL:
                part.RandomizeBarrel();
                break;
                
            case PartType.STOCK:
                part.RandomizeStock();
                break;
                
            case PartType.HANDLE:
                part.RandomizeHandle();
                break;
                
            case PartType.SIGHT:
                part.RandomizeSight();
                break;
                
            case PartType.STRING:
                part.RandomizeString();
                break;
        }

        return part;
    }
}
