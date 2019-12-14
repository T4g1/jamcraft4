using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateMachineBehaviour
{
    private Enemy enemy;

    private Animator cachedAnimator = null;
    
    public void OnTargetAquired(GameObject other)
    {
        if (other.tag == "Player") {
            cachedAnimator.SetBool("hasTarget", true);
        }
    }

    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cachedAnimator = animator;
        enemy = animator.gameObject.GetComponentInParent<Enemy>();

        enemy.SetAnimation("idle");
        enemy.AggroZone.OnZoneEnter += OnTargetAquired;
    }

    override public void OnStateExit(
        Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (enemy == null) {
            return;
        }

        enemy.AggroZone.OnZoneEnter -= OnTargetAquired;
    }
}
