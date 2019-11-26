using UnityEngine;
using UnityEngine.Assertions;

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
    public bool isSstock;
    public uint recoil;

    [Header("Sight")]
    public bool isSight;
    public uint precision;

    [Header("Handle")]
    public bool isHandle;
    public uint reloadTime;

    [Header("String")]
    public bool isString;
    public float fireRate;  // Time between every shot
    public float lifespan;  // Time before decaying
}
