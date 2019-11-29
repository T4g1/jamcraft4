using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour, IAlive
{
    [Range(0f, 1f)]
    [SerializeField]
    private float dropRate = 0.5f;

    [SerializeField]
    private TriggerZone aggroZone = null;

    [SerializeField]
    private TriggerZone lostZone = null;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private Animator behaviour = null;

    private Vector3 direction = Vector3.zero;

    [SerializeField]
    private Player player;

    // Alive interface
    [SerializeField]
    private int hitPoints;

    [SerializeField]
    private GameObject bloodInstance;

    public int HitPoints
    {
        get { return hitPoints; }
        set { hitPoints = value; }
    }

    public bool IsAlive
    {
        get { return hitPoints > 0; }
        set { }
    }


    void Start()
    {
        Assert.IsNotNull(behaviour);
        Assert.IsNotNull(aggroZone);
        Assert.IsNotNull(lostZone);

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
        if (other.tag != "Player")
        {
            return;
        }

        GameController.Instance.EnemyAggro += 1;
    }

    void OnLostTarget(GameObject other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        GameController.Instance.EnemyAggro -= 1;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        behaviour.SetBool("collisionOccured", true);
    }

    public void SetDirection(Vector3 value)
    {
        direction = value;
    }

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;

        SetAnimation("hurt");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (!IsAlive)
        {
            Die();
        }
    }

    public void Die()
    {
        if (Random.Range(0f, 1f) <= dropRate)
        {
            GameController.Instance.CreatePickUp(transform.position);
        }
        GameObject blood = Instantiate(bloodInstance, gameObject.transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
    public virtual void Attack()
    {
        // Attack the player.
    }
    public void SetAnimation(string animationName)
    {
        GetComponent<Animator>().Play(animationName);
    }
}
