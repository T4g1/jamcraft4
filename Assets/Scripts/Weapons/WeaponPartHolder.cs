using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPartHolder : MonoBehaviour
{
    [SerializeField]
    private PartType type = PartType.NONE;
    public PartType Type {
        get { return type; }
    }

    [SerializeField]
    private WeaponPart part;
    public WeaponPart Part {
        get {
            return part;
        }
        set {
            part = value;
            UpdateSpriteRenderer();
        }
    }


    void Start()
    {
        UpdateSpriteRenderer();
    }

    void UpdateSpriteRenderer()
    {
        GetComponent<SpriteRenderer>().sprite = part.sprite;
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
