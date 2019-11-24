using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Serialize]
    private Rigidbody2D body = null;

    [Serialize]
    private SpriteRenderer sprite = null;

    [Serialize]
    private Animator animator = null;

    [Serialize]
    private float speed = 8.0f;

    Vector3 moveDirection = Vector3.zero;
    string lastAnimation = "";


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
        moveDirection = new Vector3(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"), 
            0.0f
        );
        moveDirection.Normalize();
        
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
}
