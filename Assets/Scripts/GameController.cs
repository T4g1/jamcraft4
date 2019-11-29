﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    #region Weapon Sprites
    public List<Sprite> handleSprites = new List<Sprite>();
    public List<Sprite> quiverSprites = new List<Sprite>();
    public List<Sprite> stockSprites = new List<Sprite>();
    public List<Sprite> sightSprites = new List<Sprite>();
    public List<Sprite> barrelSprites = new List<Sprite>();
    public List<Sprite> stringSprites = new List<Sprite>();
    #endregion

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

    [SerializeField]
    private GameObject inventoryUI = null;
    [SerializeField]
    private GameObject craftingUI = null;

    [SerializeField]
    private TileBase floor = null;
    public TileBase Floor {
        get { return floor; }
        set {}
    }
    [SerializeField]
    private TileBase wall = null;
    public TileBase Wall {
        get { return wall; }
        set {}
    }

    [SerializeField]
    private int maxGenerations = 10;
    private int currentGeneration;

    public GameObject dynamicHolder = null;

    [SerializeField]
    private LevelGenerator levelGenerator = null;

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
        currentGeneration=0;
    }

    void Start()
    {
        Screen.SetResolution(640, 360, false);
        
        Assert.IsNotNull(wall);
        Assert.IsNotNull(floor);
        Assert.IsNotNull(intensityTween);
        Assert.IsNotNull(pickUpPrefab);
        Assert.IsNotNull(inventoryUI);
        Assert.IsNotNull(craftingUI);
        Assert.IsNotNull(levelGenerator);
        Utility.AssertArrayNotNull<Sprite>(handleSprites);
        Utility.AssertArrayNotNull<Sprite>(quiverSprites);
        Utility.AssertArrayNotNull<Sprite>(stockSprites);
        Utility.AssertArrayNotNull<Sprite>(sightSprites);
        Utility.AssertArrayNotNull<Sprite>(barrelSprites);
        Utility.AssertArrayNotNull<Sprite>(stringSprites);

        mainTheme = FMODUnity.RuntimeManager.CreateInstance(mainThemeName);
        mainTheme.start();
    }

    void Update()
    {
        mainTheme.setParameterByName(
            "intensity",
            (float) intensityTween.GetValue()
        );

        if (Input.GetButtonDown("Inventory")) {
            ToggleInventory();
        }
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
        pickUp.Item = GenerateRandomWeaponPart();
    }

    public WeaponPart GenerateRandomWeaponPart(
        PartType forcePart = PartType.NONE
    )
    {
        WeaponPart part = ScriptableObject.CreateInstance<WeaponPart>();

        PartType partType = forcePart;
        if (partType == PartType.NONE) {
            partType = (PartType) (UnityEngine.Random.Range(
                0,
                WeaponPart.GetPartTypeCount()
            ) + 1);
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

    public void OnLevelEnds()
    {
        levelGenerator.Generate();
        currentGeneration++;
    }

    public void ToggleCraftingUI()
    {
        if (craftingUI.activeSelf) {
            CloseCraftingUI();
        } else {
            OpenCraftingUI();
        }
    }

    public bool IsCraftingUIActive()
    {
        return craftingUI.activeSelf;
    }

    public void OpenCraftingUI()
    {
        craftingUI.SetActive(true);
        OpenInventory();
    }

    public void CloseCraftingUI()
    {
        if (IsCraftingUIActive()) {
            craftingUI.SetActive(false);
            CloseInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryUI.activeSelf) {
            CloseInventory();
        } else {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryUI.SetActive(false);
    }

    public void CloseUI()
    {
        CloseCraftingUI();
        CloseInventory();
    }
    public void ActivatePortal(Portal portal, Vector2 destination,bool isEnd){
        if(currentGeneration==maxGenerations && isEnd){
        portal.SetDestination(new Vector2(1000,0));
        }else{
        portal.SetDestination(destination);
        }
        portal.SetLevelEnd(isEnd);
    }

    /**
     * Instantiate a new prefab in the dynamic holder
     */
    public GameObject Instantiate(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.parent = dynamicHolder.transform;
        newObject.transform.position = position;

        return newObject;
    }
}
