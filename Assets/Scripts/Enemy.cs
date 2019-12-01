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
    public TriggerZone AggroZone
    {
        get { return aggroZone; }
        set { }
    }

    [SerializeField]
    private TriggerZone attackZone = null;
    public TriggerZone AttackZone
    {
        get { return attackZone; }
        set { }
    }

    [SerializeField]
    private TriggerZone loadingZone = null;
    public TriggerZone LoadingZone
    {
        get { return loadingZone; }
        set { }
    }

    [SerializeField]
    private TriggerZone lostZone = null;
    public TriggerZone LostZone
    {
        get { return lostZone; }
        set { }
    }

    [SerializeField]
    private float speed = 1.0f;
    
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    [SerializeField]
    private float agroSpeed = 1.0f;
    public float AgroSpeed
    {
        get { return agroSpeed; }
        set {}
    }

    [SerializeField]
    private Animator behaviour = null;

    private Vector3 direction = Vector3.zero;

    // Alive interface
    [SerializeField]
    private int hitPoints;

    [SerializeField]
    private GameObject bloodInstance = null;

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
        Assert.IsNotNull(loadingZone);
        Assert.IsNotNull(aggroZone);
        Assert.IsNotNull(lostZone);
        Assert.IsNotNull(bloodInstance);

        aggroZone.OnZoneEnter += OnGotTarget;
        lostZone.OnZoneExit += OnLostTarget;
        loadingZone.OnZoneEnter += ActivateBehaviour;
        loadingZone.OnZoneExit += DeactivateBehaviour;

        DeactivateBehaviour();
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

        loadingZone.OnZoneEnter -= ActivateBehaviour;
        loadingZone.OnZoneExit -= DeactivateBehaviour;

        GameController.Instance.OnEnemyUnloaded(this);
    }

    protected virtual void OnGotTarget(GameObject other)
    {
        // Override
    }

    protected virtual void OnLostTarget(GameObject other)
    {
        // Override
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!behaviour.gameObject.activeSelf) {
            return;
        }

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

        if (!IsAlive) {
            Die();
        }
    }

    public void Die()
    {
        if (Random.Range(0f, 1f) <= dropRate) {
            GameController.Instance.CreateRandomWeaponPartPickUp(
                transform.position
            );
        }

        GameObject blood = GameController.Instance.Instantiate(
            bloodInstance, 
            gameObject.transform.position
        );

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

    void ActivateBehaviour(GameObject other = null)
    {
        enabled = true;
        behaviour.gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().enabled = true;

        GameController.Instance.OnEnemyLoaded(this);
    }

    void DeactivateBehaviour(GameObject other = null)
    {
        enabled = false;
        behaviour.gameObject.SetActive(false);
        gameObject.GetComponent<Animator>().enabled = false;

        GameController.Instance.OnEnemyUnloaded(this);
    }
}
