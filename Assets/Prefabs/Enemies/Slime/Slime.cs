using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
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

        Utility.GetPlayer().TakeDamage(1);
    }
}
