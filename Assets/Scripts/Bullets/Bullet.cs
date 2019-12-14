using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Bullet : MonoBehaviour
{
    public float speed = 1000.0f;
    public float lifespan = 0.0f;
    public int damage = 1;

    [SerializeField]
    private Rigidbody2D body = null;

    [SerializeField]
    private GameObject destroyEffect = null;

    [SerializeField]
    private string ignoredTag = "";


    void Start()
    {
        Assert.IsNotNull(destroyEffect);
    }

    void FixedUpdate()
    {
        lifespan -= Time.fixedDeltaTime;
        if (lifespan <= 0.0f) {
            OnMiss(transform.position);
            Explode();
        }

        UpdateVelocity();
    }

    void UpdateVelocity()
    {
        body.velocity = -transform.right * speed * Time.fixedDeltaTime;
    }

    void Explode()
    {
        body.velocity *= 0;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        enabled = false;

        ParticleSystem particles = GetComponent<ParticleSystem>();
        if (particles) {
            Destroy(gameObject, particles.main.duration);
        } 
        else {
            Destroy(gameObject);
        }
    }

    /**
     * Damage every killable thing that it this
     */
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == ignoredTag) {
            return;
        }

        Vector3 collisionPosition = other.GetContact(0).point;
        OnCollision(collisionPosition);

        Alive[] alives =
            other.gameObject.GetComponents<Alive>();
        
        bool hitTarget = false;
        foreach(Alive alive in alives) {
            alive.TakeDamage(damage);
            hitTarget = true;
        }

        if (hitTarget) {
            OnHit(collisionPosition);
        } 
        else {
            OnMiss(collisionPosition);
        }
        
        Explode();
    }

    public virtual void OnCollision(Vector3 where)
    {
        Utility.Instantiate(destroyEffect, where);
    }

    public virtual void OnHit(Vector3 where)
    {
        // Override
    }

    public virtual void OnMiss(Vector3 where)
    {
        // Override
    }
}
