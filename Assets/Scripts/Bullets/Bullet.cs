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


    void Start()
    {
        Assert.IsNotNull(destroyEffect);
    }

    void FixedUpdate()
    {
        lifespan -= Time.fixedDeltaTime;
        if (lifespan <= 0.0f) {
            OnMiss();
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
        Destroy(gameObject);
    }

    /**
     * Damage every killable thing that it this
     */
    void OnCollisionEnter2D(Collision2D other) {
        OnCollision();

        MonoBehaviour[] behaviours =
            other.gameObject.GetComponents<MonoBehaviour>();
        
        bool hitTarget = false;
        foreach(MonoBehaviour behaviour in behaviours) {
            if (behaviour is IAlive) {
                IAlive killable = (IAlive) behaviour;
                killable.TakeDamage(damage);
                hitTarget = true;
            }
        }

        if (hitTarget) {
            OnHit();
        } 
        else {
            OnMiss();
        }
        
        Explode();
    }

    public virtual void OnCollision()
    {
        Utility.Instantiate(destroyEffect, transform.position);
    }

    public virtual void OnHit()
    {
        // Override
    }

    public virtual void OnMiss()
    {
        // Override
    }
}
