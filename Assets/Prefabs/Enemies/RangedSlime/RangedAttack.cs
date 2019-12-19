using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : StateMachineBehaviour
{
    private RangedSlime enemy;
    private Player player;

    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<RangedSlime>();
        player = Utility.GetPlayer();
    }

    override public void OnStateUpdate(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(
            enemy.transform.position, 
            player.transform.position
        );

        animator.SetBool("attacking", distance <= enemy.GetAttackRange());

        enemy.SetDirection(Vector3.zero);
        enemy.Attack();
    }
}
