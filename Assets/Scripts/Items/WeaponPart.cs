using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum PartType
{
    NONE = 0,
    QUIVER,
    SIGHT,
    BARREL,
    STRING,
    STOCK,
    HANDLE
}

[CreateAssetMenu(fileName = "New Part", menuName = "Weapons/New Part")]
public class WeaponPart : Item
{
    [Header("Quiver")]
    public bool isQuiver;
    public uint magazineSize;

    [Header("Barrel")]
    public bool isBarrel;
    public Bullet bulletPrefab;

    [Header("Stock")]
    public bool isStock;
    public float recoil;

    [Header("Sight")]
    public bool isSight;
    public float precision;

    [Header("Handle")]
    public bool isHandle;
    public float reloadTime;

    [Header("String")]
    public bool isString;
    public float fireRate;  // Time between every shot
    public float lifespan;  // Time before decaying

    public void RandomizeQuiver()
    {
        isQuiver = true;
        sprite = GetRandomSprite(GameController.Instance.quiverSprites);

        magazineSize = (uint) Random.Range(10, 50);
    }

    public void RandomizeBarrel()
    {
        isBarrel = true;
        sprite = GetRandomSprite(GameController.Instance.barrelSprites);

        Bullet[] bullets = Resources.LoadAll<Bullet>("Bullets");
        bulletPrefab = bullets[Random.Range(0, bullets.Length)];
    }

    public void RandomizeStock()
    {
        isStock = true;
        sprite = GetRandomSprite(GameController.Instance.stockSprites);
    }

    public void RandomizeSight()
    {
        isSight = true;
        sprite = GetRandomSprite(GameController.Instance.sightSprites);
    }

    public void RandomizeHandle()
    {
        isHandle = true;
        sprite = GetRandomSprite(GameController.Instance.handleSprites);

        reloadTime = Random.Range(0f, 5f);
    }

    public void RandomizeString()
    {
        isString = true;
        sprite = GetRandomSprite(GameController.Instance.stringSprites);

        fireRate = Random.Range(0.1f, 1f);
        lifespan = Random.Range(1f, 10f);
    }

    private Sprite GetRandomSprite(List<Sprite> sprites)
    {
        return sprites[Random.Range(0, sprites.Count)];
    }

    public override PartType GetPartType()
    {
        if (isQuiver) {
            return PartType.QUIVER;
        }
        if (isBarrel) {
            return PartType.BARREL;
        }
        if (isHandle) {
            return PartType.HANDLE;
        }
        if (isSight) {
            return PartType.SIGHT;
        }
        if (isStock) {
            return PartType.STOCK;
        }
        if (isString) {
            return PartType.STRING;
        }

        // This should never be possible
        Assert.IsTrue(false);
        return PartType.NONE;
    }

    public override bool Use()
    {
        if (!CraftingUI.Instance.IsActive()) {
            return false;
        }

        return CraftingUI.Instance.AddItem(this);
    }

    /**
     * How many different part type exists
     */
    public static int GetPartTypeCount()
    {
        return System.Enum.GetValues(typeof(PartType)).Cast<int>().Max();
    }
}
