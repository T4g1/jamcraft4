using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[ExecuteInEditMode]
public class Weapon : MonoBehaviour
{
    private Vector3 DEFAULT_SCALE = new Vector3(1, 1, 1);
    private Vector3 FLIPPED_SCALE = new Vector3(1, -1, 1);

    [SerializeField]
    private float rotationOffset = 180.0f;

    [SerializeField]
    private WeaponPart barrelPart = null;

    [SerializeField]
    private WeaponPart stockPart = null;

    [SerializeField]
    private WeaponPart sightPart = null;

    [SerializeField]
    private WeaponPart stringPart = null;

    [SerializeField]
    private WeaponPart handlePart = null;

    [SerializeField]
    private WeaponPart quiverPart = null;


    void Start()
    {
        Assert.IsNotNull(barrelPart);
        Assert.IsNotNull(stockPart);
        Assert.IsNotNull(sightPart);
        Assert.IsNotNull(stringPart);
        Assert.IsNotNull(handlePart);
        Assert.IsNotNull(quiverPart);

        transform.rotation = Quaternion.Euler(
            0.0f,
            0.0f,
            0.0f
        );

        // Place barrel left of handle
        barrelPart.SetPosition(handlePart.GetPosition() + new Vector3(
            -barrelPart.GetSize().x,
            0.0f,
            0.0f
        ));

        // Place string left of barrel
        stringPart.SetPosition(barrelPart.GetPosition() + new Vector3(
            0.0f,
            stringPart.GetSize().y / 2.0f - barrelPart.GetSize().y / 2.0f,
            0.0f
        ));

        // Place stock right of handle
        stockPart.SetPosition(handlePart.GetPosition() + new Vector3(
            handlePart.GetSize().x,
            0.0f,
            0.0f
        ));

        // Place sight above handle
        sightPart.SetPosition(handlePart.GetPosition() + new Vector3(
            -sightPart.GetSize().x / 2.0f + handlePart.GetSize().x / 2.0f,
            sightPart.GetSize().y,
            0.0f
        ));

        // Place quiver under barrel
        quiverPart.SetPosition(handlePart.GetPosition() + new Vector3(
            -quiverPart.GetSize().x,
            -barrelPart.GetSize().y,
            0.0f
        ));
    }

    void Update()
    {
        UpdateRotation();
    }

    /**
     * Look at mouse position
     */
    void UpdateRotation()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
        target.z = 0.0f;

        Vector3 origin = new Vector3(
            transform.position.x,
            transform.position.y,
            0.0f
        );

        float rotation = Mathf.Atan2(
            target.y - origin.y,
            target.x - origin.x
        );

        // To degrees
        rotation *= (180.0f / Mathf.PI);

        transform.rotation = Quaternion.Euler(
            0.0f, 
            0.0f, 
            rotationOffset + rotation
        );

        // Flip weapon
        transform.localScale = DEFAULT_SCALE;
        if (rotation < 90.0f && rotation > -90) {
            transform.localScale = FLIPPED_SCALE;
        }
    }
}
