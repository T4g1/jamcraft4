using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private TriggerZone aggroZone = null;

    [SerializeField]
    private TriggerZone lostZone = null;

    [SerializeField]
    private float speed = 1.0f;

    private Vector3 direction = Vector3.zero;


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
}
