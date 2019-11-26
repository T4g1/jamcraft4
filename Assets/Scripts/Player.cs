using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour, IAlive
{
    [SerializeField]
    private Rigidbody2D body = null;

    [SerializeField]
    private SpriteRenderer sprite = null;

    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private float speed = 200.0f;

    Vector3 moveDirection = Vector3.zero;
    string lastAnimation = "";

    // Alive interface
    [SerializeField]
    private int hitPoints;

    public int HitPoints {
        get { return hitPoints; }
        set { hitPoints = value; }
    }
    
    public bool IsAlive {
        get { return hitPoints > 0; }
        set {}
    }


    void Start() 
    {
        Assert.IsNotNull(body);
        Assert.IsNotNull(sprite);
            
        SetAnimation("idle_down");
    }

    void Update()
    {
        UpdateAnimator();
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
        Debug.Log("Not today!");
    }
}
