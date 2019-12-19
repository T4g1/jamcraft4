using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RangedSlime : Enemy
{
    [SerializeField]
    private float attackRate = 0.1f;
    [SerializeField]
    private float bulletLifespan = 3.0f;

    private float attackCooldown = 0.0f;

    [SerializeField]
    private Bullet bulletPrefab = null;


    protected override void Start()
    {
        Assert.IsNotNull(bulletPrefab);

        base.Start();
    }

    public override void Attack()
    {
        Player player = Utility.GetPlayer();
        float distance = Vector3.Distance(
            player.gameObject.transform.position,
            gameObject.transform.position
        );

        // Check player is in range
        if (distance > GetAttackRange()) {
            return;
        }

        attackCooldown -= Time.deltaTime;
        if (attackCooldown > 0) {
            return;
        }

        attackCooldown = attackRate;

        Bullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = gameObject.transform.position;
        bullet.lifespan = bulletLifespan;

        // Get direction of the player
        float rotationRaw = Mathf.Atan2(
            player.transform.position.y - gameObject.transform.position.y,
            player.transform.position.x - gameObject.transform.position.x
        );
        
        // To degrees
        rotationRaw *= (180.0f / Mathf.PI);
        
        bullet.transform.rotation = Quaternion.Euler(
            0.0f,
            0.0f,
            180 + rotationRaw
        );
    }
}
