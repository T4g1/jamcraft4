using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : StateMachineBehaviour
{
    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy enemy = animator.gameObject.GetComponentInParent<Enemy>();
        enemy.SetAnimation("attack");
        enemy.SetDirection(Vector3.zero);
    }
}
