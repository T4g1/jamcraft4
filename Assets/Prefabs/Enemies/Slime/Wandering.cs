using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : StateMachineBehaviour
{
    private Enemy enemy;
    private float directionChangeDelay;

    [SerializeField]
    private float minDirectionTime = 2.0f;

    private Animator cachedAnimator = null;

    [SerializeField]
    private float maxDirectionTime = 5.0f;
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
        enemy = animator.gameObject.GetComponentInParent<Enemy>(
            typeof(Enemy), 
            true
        );

        directionChangeDelay = 0.0f;

        enemy.SetAnimation("walk");
        enemy.AggroZone.OnZoneEnter += OnTargetAquired;
    }

    override public void OnStateUpdate(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        directionChangeDelay -= Time.deltaTime;
        if (directionChangeDelay <= 0) {
            changeDirection();
        }

        if (animator.GetBool("collisionOccured")) {
            animator.SetBool("collisionOccured", false);

            changeDirection();
        }
    }

    override public void OnStateExit(
        Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (enemy == null) {
            return;
        }
        
        enemy.AggroZone.OnZoneEnter -= OnTargetAquired;
    }

    /**
     * Change direction of movement
     */
    void changeDirection()
    {
        directionChangeDelay = Random.Range(minDirectionTime, maxDirectionTime);
        Vector3 direction = new Vector3(
            Random.Range(-1.0f, 1.0f),
            Random.Range(-1.0f, 1.0f),
            0.0f
        );
        direction.Normalize();

        enemy.SetDirection(direction);
    }
}
