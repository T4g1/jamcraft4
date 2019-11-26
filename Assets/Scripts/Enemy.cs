using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IAlive
{
    [SerializeField]
    private TriggerZone aggroZone = null;

    [SerializeField]
    private TriggerZone lostZone = null;

    [SerializeField]
    private float speed = 1.0f;

    private Vector3 direction = Vector3.zero;

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
        aggroZone.OnZoneEnter += OnGotTarget;
        lostZone.OnZoneExit += OnLostTarget;
    }

    void Update()
    {
        UpdateMovement();
    }

    /**
     * Move the enemy
     */
    void UpdateMovement()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnDestroy() 
    {
        aggroZone.OnZoneEnter -= OnGotTarget;
        lostZone.OnZoneExit -= OnLostTarget;
    }

    void OnGotTarget(GameObject other)
    {
        if (other.tag != "Player") {
            return;
        }

        GameController.Instance.EnemyAggro += 1;
    }

    void OnLostTarget(GameObject other)
    {
        if (other.tag != "Player") {
            return;
        }

        GameController.Instance.EnemyAggro -= 1;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        GetComponent<Animator>().SetBool("collisionOccured", true);
    }

    public void SetDirection(Vector3 value)
    {
        direction = value;
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
        Destroy(gameObject);
    }
}
