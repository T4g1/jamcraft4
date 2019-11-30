using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour, IAlive
{
    public event System.Action<int> OnHitPointsChanged;

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

    // Alive interface
    [SerializeField]
    private int maxHitPoints = 3;
    private int hitPoints = 0;

    public int HitPoints {
        get { return hitPoints; }
        set { 
            hitPoints = value; 

            if (OnHitPointsChanged != null) {
                OnHitPointsChanged(hitPoints);
            }
        }
    }
    
    public bool IsAlive {
        get { return hitPoints > 0; }
        set {}
    }
    
    [SerializeField]
    private Tooltip reloadUI = null;


    void Start() 
    {
        Assert.IsNotNull(reloadUI);
        Assert.IsNotNull(body);
        Assert.IsNotNull(sprite);
        Assert.IsNotNull(cameraContainer);
        
        SetAnimation("idle_down");

        HitPoints = maxHitPoints;
        
        Utility.GetWeapon().OnMagazineEmpty += OnMagazineEmpty;
        Utility.GetWeapon().OnReloading += OnReloading;
    }

    void OnDestroy()
    {
        Weapon weapon = Utility.GetWeapon();
        if (weapon != null) {
            weapon.OnMagazineEmpty -= OnMagazineEmpty;
            weapon.OnReloading -= OnReloading;
        }
    }

    void Update()
    {
        UpdateAnimator();
    }

    void Spawn()
    {
        GetCamera().enabled = true;
    }

    void FixedUpdate()
    {
        UpdateVelocity();
    }

    void UpdateVelocity()
    {
        moveDirection = new Vector3(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"), 
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
    
    public void TakeDamage(int amount)
    {
        hitPoints -= amount;

        if (!IsAlive) {
            Die();
        }
    }

    public void Die()
    {
        // TODO
    }

    public Camera GetCamera()
    {
        return cameraContainer.GetComponent<Camera>();
    }

    public void OnReloading()
    {
        reloadUI.Hide();
    }

    public void OnMagazineEmpty()
    {
        reloadUI.Show();
    }
}
