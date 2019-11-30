using System;
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
            Debug.Log("More than one GameController created!");
        }

        Instance = this;
    }
    #endregion

    private FMOD.Studio.EventInstance theme;

    public const uint TILE_SIZE = 16;

    [FMODUnity.EventRef]
    public string themeName;

    [SerializeField]
    private Tween intensityTween = null;

    [SerializeField]
    private GameObject pickUpPrefab = null;
    
    [SerializeField]
    private Transform lastRoomEntry = null;

    [SerializeField]
    private float deathTime = 2.0f;

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
    private uint lastLevel = 5;
    private uint currentLevel = 0;

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
            UpdateThemeIntensity();
        }
    }


    private void Awake() {
        InitInstance();
    }

    void OnFocus()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        
        Assert.IsNotNull(wall);
        Assert.IsNotNull(floor);
        Assert.IsNotNull(intensityTween);
        Assert.IsNotNull(pickUpPrefab);
        Assert.IsNotNull(levelGenerator);
        Assert.IsNotNull(lastRoomEntry);
        Utility.AssertArrayNotNull<Sprite>(handleSprites);
        Utility.AssertArrayNotNull<Sprite>(quiverSprites);
        Utility.AssertArrayNotNull<Sprite>(stockSprites);
        Utility.AssertArrayNotNull<Sprite>(sightSprites);
        Utility.AssertArrayNotNull<Sprite>(barrelSprites);
        Utility.AssertArrayNotNull<Sprite>(stringSprites);

        theme = FMODUnity.RuntimeManager.CreateInstance(themeName);
        theme.start();

        Utility.GetPlayer().OnDeath += OnPlayerDies;
    }

    void Update()
    {
        theme.setParameterByName(
            "intensity",
            (float) intensityTween.GetValue()
        );
    }

    void UpdateThemeIntensity()
    {
        int intensity = (int)(enemyAggro * 100.0f / enemyCount);

        intensityTween.Interpolate(intensityTween.GetValue(), intensity);
    }

    /**
     * Generate a random weapon part that can be picked up
     */
    public void CreateRandomWeaponPartPickUp(Vector3 where)
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

    /**
     * Teleports player back to spawn and resplenish life
     */
    public void OnPlayerDies()
    {
        StartCoroutine(_OnPlayerDies());
    }

    IEnumerator _OnPlayerDies()
    {
        yield return new WaitForSeconds(deathTime);

        Player player = Utility.GetPlayer();
        player.transform.position = levelGenerator.Spawn.position;
        player.Heal();
    }

    public void OnLevelEnds()
    {
        levelGenerator.Generate();
    }

    public void OnLevelGenerated()
    {
        currentLevel += 1;

        if (currentLevel >= lastLevel) {
            levelGenerator.PortalOut.SetDestination(
                lastRoomEntry.position
            );
        }
        
        levelGenerator.PortalIn.Activate();
        levelGenerator.PortalOut.Activate();    // TODO: Do this when boss dies
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

    public void OnDestroy()
    {
        theme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
        Player player = Utility.GetPlayer();
        if (player) {
            player.OnDeath -= OnPlayerDies;
        }
    }
}
