using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

//[ExecuteInEditMode] // Uncomment to update weapon part position in Editor
public class Weapon : MonoBehaviour
{
    // Display settings
    [SerializeField]
    private Vector3 defaultScale = new Vector3(1, 1, 1);
    private Vector3 flippedScale;

    [SerializeField]
    private float rotationOffset = 180.0f;

    // Weapon part holders
    [SerializeField]
    private WeaponPartHolder barrelHolder = null;

    [SerializeField]
    private WeaponPartHolder stockHolder = null;

    [SerializeField]
    private WeaponPartHolder sightHolder = null;

    [SerializeField]
    private WeaponPartHolder stringHolder = null;

    [SerializeField]
    private WeaponPartHolder handleHolder = null;

    [SerializeField]
    private WeaponPartHolder quiverHolder = null;

    private Camera playerCamera;        // Cache player camera

    // Shooting system
    [SerializeField]
    private GameObject muzzle = null;

    private float shotCooldown = 0.0f;  // Time before next bullet
    private uint magazineClip = 0;      // Bullet left
    private bool reloading = false;
    private float reloadTime = 0.0f;    // Time before reload complete


    void Start()
    {
        Assert.IsNotNull(barrelHolder);
        Assert.IsNotNull(stockHolder);
        Assert.IsNotNull(sightHolder);
        Assert.IsNotNull(stringHolder);
        Assert.IsNotNull(handleHolder);
        Assert.IsNotNull(quiverHolder);
        
        magazineClip = GetMagazineSize();

        playerCamera = Camera.main;

        flippedScale = new Vector3(
            defaultScale.x,
            defaultScale.y * -1,
            defaultScale.z
        );

        transform.rotation = Quaternion.Euler(
            0.0f,
            0.0f,
            0.0f
        );

        // Place barrel left of handle
        barrelHolder.SetPosition(handleHolder.GetPosition() + new Vector3(
            -barrelHolder.GetSize().x,
            0.0f,
            0.0f
        ));

        // Place string left of barrel
        stringHolder.SetPosition(barrelHolder.GetPosition() + new Vector3(
            0.0f,
            stringHolder.GetSize().y / 2.0f - barrelHolder.GetSize().y / 2.0f,
            0.0f
        ));

        // Place stock right of handle
        stockHolder.SetPosition(handleHolder.GetPosition() + new Vector3(
            handleHolder.GetSize().x,
            0.0f,
            0.0f
        ));

        // Place sight above handle
        sightHolder.SetPosition(handleHolder.GetPosition() + new Vector3(
            -sightHolder.GetSize().x / 2.0f + handleHolder.GetSize().x / 2.0f,
            sightHolder.GetSize().y,
            0.0f
        ));

        // Place quiver under barrel
        quiverHolder.SetPosition(handleHolder.GetPosition() + new Vector3(
            -quiverHolder.GetSize().x,
            -barrelHolder.GetSize().y,
            0.0f
        ));

        // Place muzzle at the end of barrel
        muzzle.transform.position = barrelHolder.GetPosition() + new Vector3(
            0.0f,
            -barrelHolder.GetSize().y / 2,
            0.0f
        );
    }

    void Update()
    {
        shotCooldown = Mathf.Max(0, shotCooldown -Time.deltaTime);
        reloadTime = Mathf.Max(0, reloadTime -Time.deltaTime);

        if (reloading && reloadTime <= 0) {
            reloading = false;
            magazineClip = GetMagazineSize();
            Debug.Log("Reloaded");
        }
        
        UpdateRotation();

        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        HandleInputs();
    }

    /**
     * Look at mouse position
     */
    void UpdateRotation()
    {
        Vector3 target = playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
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
        transform.localScale = defaultScale;
        if (rotation < 90.0f && rotation > -90) {
            transform.localScale = flippedScale;
        }
    }

    /**
     * Handle player inputs
     */
    void HandleInputs()
    {
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
        
        if (Input.GetButtonDown("Reload")) {
            Reload();
        }
    }

    /**
     * Shoot a projectile
     */
    void Shoot()
    {
        if (shotCooldown > 0) {
            return;
        }

        if (magazineClip <= 0) {
            return;
        }

        Bullet bullet = Instantiate(GetBulletPrefab());
        bullet.transform.position = GetMuzzlePosition();
        bullet.transform.rotation = transform.rotation;
        bullet.lifespan = GetBulletLifeSpan();

        magazineClip -= 1;

        shotCooldown = GetShotInterval();
    }

    void Reload()
    {
        if (reloading) {
            return;
        }

        Debug.Log("Reloading");
        reloading = true;
        reloadTime = GetReloadTime();
    }

    /**
     * Give time between every shoot
     */
    float GetShotInterval()
    {
        return stringHolder.Part.fireRate;
    }

    /**
     * Get bullet used when shooting
     */
    Bullet GetBulletPrefab()
    {
        return barrelHolder.Part.bulletPrefab;
    }

    /**
     * Where are arrows coming from
     */
    Vector3 GetMuzzlePosition()
    {
        return muzzle.transform.position;
    }

    /**
     * Time before bullet decays
     */
    float GetBulletLifeSpan()
    {
        return stringHolder.Part.lifespan;
    }

    /**
     * Get reload time
     */
    float GetReloadTime()
    {
        return handleHolder.Part.reloadTime;
    }

    /**
     * Size of magazine clip
     */
    uint GetMagazineSize()
    {
        return quiverHolder.Part.magazineSize;
    }
}
