using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : StateMachineBehaviour
{
    private Enemy enemy;
    private Animator cachedAnimator = null;
    
    private float attackDelay = 0.5f;
    private float attackCooldown = 0f;

    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cachedAnimator = animator;
        enemy = animator.gameObject.GetComponentInParent<Enemy>();

        enemy.SetAnimation("attack");
        enemy.SetDirection(Vector3.zero);

        attackCooldown = attackDelay;
    }

    override public void OnStateUpdate(
        Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
    {
        enemy.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0) {
            cachedAnimator.SetBool("attacking", false);
        }
    }
}
