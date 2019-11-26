using UnityEngine;
using UnityEngine.Assertions;

[ExecuteInEditMode]
public class WeaponPart : Item
{
    public PartType partType;
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


    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    /**
     * Get position of upper left corner of sprite used
     */
    public Vector3 GetPosition()
    {
        return transform.position + GetCenterOffset();
    }

    /**
     * Set position of upper left corner of sprite used
     */
    public void SetPosition(Vector3 value)
    {
        transform.position = value - GetCenterOffset();
    }

    /**
     * Gives offset to convert from center position to upper left position
     */
    private Vector3 GetCenterOffset()
    {
        return new Vector3(
            -GetSize().x / 2.0f,
            GetSize().y / 2.0f,
            0.0f
        );
    }

    /**
     * Get size of the sprite used
     */
    public Vector2 GetSize()
    {
        Vector2 scale = new Vector2(
            transform.lossyScale.x,
            transform.lossyScale.y
        );
        
        return GetComponent<SpriteRenderer>().sprite.bounds.size * scale;
    }
}

public enum PartType
{
    QUIVER,
    SIGHT,
    BARREL,
    STRING,
    STOCK,
    HANDLE
}
