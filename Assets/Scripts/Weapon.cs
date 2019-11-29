using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

//[ExecuteInEditMode] // Uncomment to update weapon part position in Editor
public class Weapon : MonoBehaviour
{
    public event System.Action<uint> OnMagazineClipChanged;
    public event System.Action OnShoot;
    public event System.Action OnMagazineEmpty;

    // Display settings
    [SerializeField]
    private Vector3 defaultScale = new Vector3(1, 1, 1);
    private Vector3 flippedScale;

    [SerializeField]
    private float rotationOffset = 180.0f;
    private float rotationRaw;
    [SerializeField]
    private float maximalPerturbation = 180.0f; // Max offset added by precision

    private WeaponPartHolder[] partHolders;

    private Camera playerCamera;        // Cache player camera

    // Shooting system
    [SerializeField]
    private float visorSpeed = 10.0f;
    [SerializeField]
    private GameObject visor = null;
    [SerializeField]
    private GameObject muzzle = null;

    private float shotCooldown = 0.0f;  // Time before next bullet
    private bool reloading = false;
    private float reloadTime = 0.0f;    // Time before reload complete
    private uint magazineClip = 0;      // Bullet left
    private uint MagazineClip {
        get { return magazineClip; }
        set {
            magazineClip = value;

            if (OnMagazineClipChanged != null) {
                OnMagazineClipChanged(magazineClip);
            }
        }
    }
    
    private WeaponPartHolder barrelHolder;
    private WeaponPartHolder stockHolder;
    private WeaponPartHolder sightHolder;
    private WeaponPartHolder stringHolder;
    private WeaponPartHolder handleHolder;
    private WeaponPartHolder quiverHolder;

    // FMOD declare emitter instance
    FMODUnity.StudioEventEmitter sfx;
    const int SFX_SHOOT_FULL = 1;
    const int SFX_SHOOT_EMPTY = 0;


    void Start()
    {
        Assert.IsNotNull(visor);
        Assert.IsNotNull(muzzle);

        partHolders = gameObject.GetComponentsInChildren<WeaponPartHolder>();
        
        Assert.IsNotNull(partHolders);
        Assert.IsTrue(partHolders.Length == WeaponPart.GetPartTypeCount());

        barrelHolder = GetHolder(PartType.BARREL);
        stockHolder = GetHolder(PartType.STOCK);
        sightHolder = GetHolder(PartType.SIGHT);
        stringHolder = GetHolder(PartType.STRING);
        handleHolder = GetHolder(PartType.HANDLE);
        quiverHolder = GetHolder(PartType.QUIVER);

        Assert.IsNotNull(barrelHolder);
        Assert.IsNotNull(stockHolder);
        Assert.IsNotNull(sightHolder);
        Assert.IsNotNull(stringHolder);
        Assert.IsNotNull(handleHolder);
        Assert.IsNotNull(quiverHolder);
        
        MagazineClip = GetMagazineSize();
        
        // FMOD connect emitter to event emitter component
        sfx = GetComponent<FMODUnity.StudioEventEmitter>();
        Debug.Log("SFX_SHOOT_FULL");
        sfx.SetParameter("IsEmpty", 0);

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

        UpdateLayout();
    }

    /**
     * Compute position of every part of the weapon
     */
    public void UpdateLayout()
    {
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
        // Update cooldowns
        shotCooldown = Mathf.Max(0, shotCooldown -Time.deltaTime);
        reloadTime = Mathf.Max(0, reloadTime -Time.deltaTime);

        if (reloading && reloadTime <= 0) {
            Debug.Log("SFX_SHOOT_FULL");
            sfx.SetParameter("IsEmpty", 0);

            reloading = false;
            MagazineClip = GetMagazineSize();
        }

        if (EventSystem.current.IsPointerOverGameObject()) {
            Cursor.visible = true;
            return;
        } 
        else {
            Cursor.visible = false;
        }
        
        UpdateVisor();
        UpdateRotation();
        HandleInputs();
    }

    void UpdateVisor()
    {
        visor.transform.rotation = Quaternion.identity;
        visor.transform.position =  Vector3.Lerp(
            visor.transform.position, 
            Utility.GetMouseWorldPosition(), 
            Time.deltaTime * visorSpeed
        );
    }

    /**
     * Look at mouse position
     */
    void UpdateRotation()
    {
        Vector3 target = visor.transform.position;
        target.z = 0.0f;

        Vector3 origin = new Vector3(
            transform.position.x,
            transform.position.y,
            0.0f
        );

        rotationRaw = Mathf.Atan2(
            target.y - origin.y,
            target.x - origin.x
        );

        // To degrees
        rotationRaw *= (180.0f / Mathf.PI);

        transform.rotation = Quaternion.Euler(
            0.0f, 
            0.0f, 
            GetCurrentRotation()
        );

        // Flip weapon
        transform.localScale = defaultScale;
        if (rotationRaw < 90.0f && rotationRaw > -90) {
            transform.localScale = flippedScale;
        }
    }

    /**
     * Handle player inputs
     */
    void HandleInputs()
    {
        if (Input.GetButton("Fire1")) {
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

        shotCooldown = GetShotInterval();

        if (MagazineClip <= 0) {
            Debug.Log("PLAY");
            sfx.Play();
            return;
        }

        Bullet bullet = Instantiate(GetBulletPrefab());
        bullet.transform.position = GetMuzzlePosition();
        bullet.transform.rotation = GetShootDirection();
        bullet.lifespan = GetBulletLifeSpan();

        Vector3 recoil = new Vector3(
            Random.Range(0f, GetRecoil()), 
            Random.Range(0f, GetRecoil()), 
            0f
        );

        visor.transform.position += recoil;

        MagazineClip -= 1;
        Debug.Log("PLAY");
        sfx.Play();

        if (OnShoot != null) {
            OnShoot();
        }

        if (MagazineClip <= 0) {
            Debug.Log("SFX_SHOOT_EMPTY");
            sfx.SetParameter("IsEmpty", 1);

            if (OnMagazineEmpty != null) {
                OnMagazineEmpty();
            }
        }
    }

    void Reload()
    {
        if (reloading) {
            return;
        }

        reloading = true;
        reloadTime = GetReloadTime();
    }

    Quaternion GetShootDirection()
    {
        float perturbation = maximalPerturbation * GetPrecision();
        perturbation = Random.Range(
            -perturbation/2, 
            perturbation/2
        );

        return Quaternion.Euler(
            0.0f,
            0.0f,
            GetCurrentRotation() + perturbation
        );
    }

    float GetCurrentRotation()
    {
        return rotationOffset + rotationRaw;
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
     * Get recoil
     */
    float GetRecoil()
    {
        return stockHolder.Part.recoil;
    }

    /**
     * Get precision
     */
    float GetPrecision()
    {
        return sightHolder.Part.precision;
    }

    /**
     * Size of magazine clip
     */
    uint GetMagazineSize()
    {
        return quiverHolder.Part.magazineSize;
    }

    /**
     * Set a part of the weapon
     */
    public void SetPart(WeaponPart part)
    {
        WeaponPartHolder holder = GetHolder(part.GetPartType());
        holder.Part = part;

        UpdateLayout();
    }

    WeaponPartHolder GetHolder(PartType type)
    {
        foreach (WeaponPartHolder holder in partHolders) {
            if (holder.Type == type) {
                return holder;
            }
        }

        // This should never happen. If this happen, Weapon prefab is missing
        // one PartType holder (or something requested a NONE part holder)
        Assert.IsTrue(false);
        return null;
    }
}
