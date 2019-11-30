using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : StateMachineBehaviour
{
    private Enemy enemy;
    private Animator cachedAnimator = null;

    private float oldSpeed;


    public void OnTargetLost(GameObject other)
    {
        if (other.tag == "Player") {
            cachedAnimator.SetBool("hasTarget", false);
        }
    }

    public void OnTargetAquired(GameObject other)
    {
        if (other.tag == "Player") {
            cachedAnimator.SetBool("attacking", true);
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
        enemy.AttackZone.OnZoneEnter += OnTargetAquired;
    }

    override public void OnStateUpdate(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FollowPlayer();
    }

    override public void OnStateExit(
        Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        enemy.Speed = oldSpeed;

        enemy.LostZone.OnZoneExit -= OnTargetLost;
        enemy.AttackZone.OnZoneEnter -= OnTargetAquired;
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
