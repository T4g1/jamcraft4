using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField]
    private float attackRange = 1.0f;
    public float AttackRange {
        get { return attackRange; }
        set {}
    }

    public override void Attack()
    {
        Player player = Utility.GetPlayer();
        float distance = Vector3.Distance(
            player.gameObject.transform.position,
            gameObject.transform.position
        );

        // Check player is in range
        if (distance > attackRange) {
            return;
        }

        Utility.GetPlayer().TakeDamage(1);
    }
}
