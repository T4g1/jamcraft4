using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Player : Alive
{
    [SerializeField] 
    private UnityEvent onPlayerLoaded = null;

    [SerializeField]
    private Rigidbody2D body = null;

    [SerializeField]
    private SpriteRenderer sprite = null;

    [SerializeField]
    private GameObject cameraContainer = null;

    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private float speed = 200.0f;

    Vector3 moveDirection = Vector3.zero;
    string lastAnimation = "";

    private Weapon weapon;
    
    [SerializeField]
    private Tooltip reloadUI = null;
    
    [FMODUnity.EventRef]
    public string walkSFX = "";

    [Range(0f, 1f)]
    [SerializeField] 
    private float invulnerabilityOpacity = 0.5f;
    [SerializeField] 
    private float invulnerabilityTime = 2.0f;
    private bool invulnerable;
    private float invulnerabilityTimer = 0.0f;


    protected override void Start()
    {
        Assert.IsNotNull(reloadUI);
        Assert.IsNotNull(body);
        Assert.IsNotNull(sprite);
        Assert.IsNotNull(cameraContainer);

        Assert.IsTrue(walkSFX != "");
        
        SetAnimation("idle_down");
        
        Utility.GetWeapon().OnMagazineEmpty += OnMagazineEmpty;
        Utility.GetWeapon().OnReloading += OnReloading;

        weapon = Utility.GetWeapon();

        onPlayerLoaded.Invoke();

        base.Start();
    }

    void OnDestroy()
    {
        if (weapon != null) {
            weapon.OnMagazineEmpty -= OnMagazineEmpty;
            weapon.OnReloading -= OnReloading;
        }
    }

    /**
     * Wether or not player is directly controlled right now
     */
    public bool InputEnabled()
    {
        return IsAlive && !Cursor.visible;
    }

    void Update()
    {
        if (InputEnabled()) {
            UpdateAnimator();

            UpdateInvulnerability();

            weapon.UpdateVisor();
            weapon.UpdateRotation();

            HandleInputs();
        }
    }

    void Spawn()
    {
        GetCamera().enabled = true;
    }

    void FixedUpdate()
    {
        if (InputEnabled()) {
            UpdateVelocity();
        }
    }

    void HandleInputs()
    {
        if (InputController.Instance.GetButton("shoot")) {
            weapon.Shoot();
        }
        
        if (InputController.Instance.GetButtonDown("reload")) {
            weapon.Reload();
        }
    }

    public void UpdateInvulnerability()
    {
        if (!invulnerable) {
            return;
        }

        invulnerabilityTimer = Mathf.Max(
            0, 
            invulnerabilityTimer -Time.deltaTime
        );
        
        if (invulnerabilityTimer <= 0.0f) {
            DisableInvulnerability();
        }
    }

    public void StopMovement()
    {
        body.velocity = Vector3.zero;
    }

    void UpdateVelocity()
    {
        moveDirection = new Vector3(
            InputController.Instance.GetHorizontalAxis(), 
            InputController.Instance.GetVerticalAxis(), 
            0.0f
        );
        moveDirection.Normalize();

        // TODO: Overriding velocity value will remove any other force 
        // applied to the body. Fix this behavior if outside forces needed
        
        body.velocity = moveDirection * speed * Time.fixedDeltaTime;
    }

    void UpdateAnimator()
    {
        if (moveDirection.y < 0.0f) {
            sprite.flipX = false;
            SetAnimation("walk_down");
        }
        else if (moveDirection.y > 0.0f) {
            sprite.flipX = false;
            SetAnimation("walk_up");
        }
        else if (moveDirection.x > 0.0f) {
            sprite.flipX = false;
            SetAnimation("walk_right");
        }
        else if (moveDirection.x < 0.0f) {
            sprite.flipX = true;
            SetAnimation("walk_right");
        }

        else if (lastAnimation == "walk_up") {
            sprite.flipX = false;
            SetAnimation("idle_up");
        }
        else if (lastAnimation == "walk_down") {
            sprite.flipX = false;
            SetAnimation("idle_down");
        }
        else if (lastAnimation == "walk_right") {
            SetAnimation("idle_right");
        }
    }

    void SetAnimation(string stateName)
    {
        if (animator == null) {
            return;
        }
        
        animator.Play(stateName);
        lastAnimation = stateName;
    }

    public override void Heal()
    {
        if (!IsAlive) {
            SetAnimation("idle_down");
        }
        
        HitPoints = maxHitPoints;
    }

    public override void TakeDamage(int amount)
    {
        if (invulnerable) {
            return;
        }

        ActivateInvulnerability();

        base.TakeDamage(amount);
    }

    public override void Die()
    {
        StopMovement();
        SetAnimation("die");
        DisableInvulnerability();
        
        base.Die();
    }

    public Camera GetCamera()
    {
        return cameraContainer.GetComponent<Camera>();
    }

    public void OnReloading()
    {
        //reloadUI.Hide();
    }

    public void OnMagazineEmpty()
    {
        //reloadUI.Show();
    }

    void ActivateInvulnerability()
    {
        if (!IsAlive) {
            return;
        }

        invulnerable = true;
        invulnerabilityTimer = invulnerabilityTime;

        Color color = sprite.color;
        color.a = invulnerabilityOpacity;
        sprite.color = color;
    }

    void DisableInvulnerability()
    {
        invulnerable = false;
        
        Color color = sprite.color;
        color.a = 1.0f;
        sprite.color = color;
    }
}
