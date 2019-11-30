using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : StateMachineBehaviour
{
    private Enemy enemy;
    private Animator Animator = null;


    public void OnTargetLost(GameObject other)
    {
        if (other.tag == "Player")
        {
            Animator.SetBool("attacking", false);
        }
    }



    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<Enemy>();

        Animator = animator;
        enemy.SetAnimation("attack");
        enemy.AttackZone.OnZoneExit += OnTargetLost;

        Utility.GetPlayer().TakeDamage(1);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        keepDirection();
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        enemy.AttackZone.OnZoneExit -= OnTargetLost;
    }
    private void keepDirection(){
        enemy.SetDirection(Vector3.zero);
    }


}
