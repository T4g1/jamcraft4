using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : StateMachineBehaviour
{
    private Player player;
    private Enemy enemy;
    private Animator cachedAnimator = null;

    private float oldSpeed;

    public void OnTargetLost(GameObject other)
    {
        if (other.tag == "Player") {
            cachedAnimator.SetBool("hasTarget", false);
        }
    }


    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cachedAnimator = animator;
        enemy = animator.gameObject.GetComponentInParent<Enemy>();

        enemy.SetAnimation("walk");

        oldSpeed = enemy.Speed;
        enemy.Speed = enemy.AgroSpeed;

        enemy.LostZone.OnZoneExit += OnTargetLost;

        player = Utility.GetPlayer();
    }

    override public void OnStateUpdate(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FollowPlayer();

        float distance = Vector3.Distance(
            enemy.transform.position, 
            player.transform.position
        );

        cachedAnimator.SetBool("attacking", distance <= enemy.GetAttackRange());
    }

    override public void OnStateExit(
        Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        enemy.Speed = oldSpeed;

        enemy.LostZone.OnZoneExit -= OnTargetLost;
    }

    /**
     * Move towards player
     */
    void FollowPlayer()
    {
        Vector3 playerPosition = Utility.GetPlayer().transform.position;

        Vector3 direction = playerPosition - enemy.transform.position;
        direction.Normalize();

        enemy.SetDirection(direction);
    }
}
