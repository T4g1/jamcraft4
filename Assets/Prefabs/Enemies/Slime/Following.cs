using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : StateMachineBehaviour
{
    private Enemy enemy;
    private float directionChangeDelay;

    [SerializeField]
    private float minDirectionTime = 2.0f;

    [SerializeField]
    private float maxDirectionTime = 5.0f;

    private Animator Animator=null;

    public void OnTargetLost(GameObject other){
        if(other.tag=="Player"){
            // Check if that way is a good way to access the Animator.
            Animator.SetBool("hasTarget",false);
        }
    }



    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        enemy = animator.gameObject.GetComponentInParent<Enemy>();
        directionChangeDelay = 0.0f;
        Animator=animator;
        enemy.SetAnimation("walk");
        enemy.LostZone.OnZoneExit+=OnTargetLost;
    }
    
    override public void OnStateUpdate(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        changeDirection();
            //TODO: Determine what to do on Collission.
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        enemy.LostZone.OnZoneExit-=OnTargetLost;
        
        
    }
    /**
     * Change direction of movement
     */
    void changeDirection()
    {
        Vector3 direction = Utility.GetPlayer().transform.position;
        Debug.Log(direction);

        direction.Normalize();

        enemy.SetDirection(direction);
    }

    
}
