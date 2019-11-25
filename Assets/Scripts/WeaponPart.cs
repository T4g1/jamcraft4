using UnityEngine;
using UnityEngine.Assertions;

[ExecuteInEditMode]
public class WeaponPart : MonoBehaviour
{
    public PartType partType;
    public Sprite sprite;

    [Header("Quiver")]
    public bool quiver;
    public uint size;

    [Header("Barrel")]
    public bool barrel;
    public uint fireRate;

    [Header("Stock")]
    public bool stock;
    public uint recoil;

    // TODO: Other systems


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
        return GetComponent<SpriteRenderer>().sprite.bounds.size;
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
