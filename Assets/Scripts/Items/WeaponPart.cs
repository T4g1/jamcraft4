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
    public Sprite sprite;

    [Header("Quiver")]
    public bool isQuiver;
    public uint magazineSize;

    [Header("Barrel")]
    public bool isBarrel;
    public Bullet bulletPrefab;

    [Header("Stock")]
    public bool isStock;
    public uint recoil;

    [Header("Sight")]
    public bool isSight;
    public uint precision;

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
        magazineSize = (uint) Random.Range(10, 50);
    }

    public void RandomizeBarrel()
    {
        isBarrel = true;

        Bullet[] bullets = Resources.LoadAll<Bullet>("Bullets");
        bulletPrefab = bullets[Random.Range(0, bullets.Length)];
    }

    public void RandomizeStock()
    {
        isStock = true;
    }

    public void RandomizeSight()
    {
        isSight = true;
    }

    public void RandomizeHandle()
    {
        isHandle = true;
        reloadTime = Random.Range(0f, 5f);
    }

    public void RandomizeString()
    {
        isString = true;
        fireRate = Random.Range(0.1f, 1f);
        lifespan = Random.Range(1f, 10f);
    }
}
