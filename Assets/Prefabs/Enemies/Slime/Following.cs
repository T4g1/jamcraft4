using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : StateMachineBehaviour
{
    private Enemy enemy;
    private Animator Animator = null;

    private float oldSpeed;

    public void OnTargetLost(GameObject other)
    {
        if (other.tag == "Player")
        {
            // Check if that way is a good way to access the Animator.
            Animator.SetBool("hasTarget", false);
            enemy.Speed=oldSpeed;
        }
    }

    public void OnTargetAquired(GameObject other)
    {
        if (other.tag == "Player")
        {
            Animator.SetBool("attacking", true);
        }
    }



    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<Enemy>();

        Animator = animator;
        enemy.SetAnimation("walk");

        oldSpeed=enemy.Speed;
        enemy.Speed=enemy.AgroSpeed;

        enemy.LostZone.OnZoneExit += OnTargetLost;
        enemy.AttackZone.OnZoneEnter += OnTargetAquired;
    }

    override public void OnStateUpdate(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        changeDirection();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        enemy.LostZone.OnZoneExit -= OnTargetLost;
        enemy.AttackZone.OnZoneEnter -= OnTargetAquired;


    }
    /**
     * Change direction of movement
     */
    void changeDirection()
    {
        Vector3 playerPos=Utility.GetPlayer().transform.position;
        Vector3 direction = playerPos-enemy.transform.position;
        direction.Normalize();
        enemy.SetDirection(direction);
    }


}
