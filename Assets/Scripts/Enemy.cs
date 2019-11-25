using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private TriggerZone aggroZone = null;

    [SerializeField]
    private TriggerZone lostZone = null;


    void Start()
    {
        aggroZone.OnZoneEnter += OnGotTarget;
        lostZone.OnZoneExit += OnLostTarget;
    }

    void OnDestroy() 
    {
        aggroZone.OnZoneEnter -= OnGotTarget;
        lostZone.OnZoneExit -= OnLostTarget;
    }

    void OnGotTarget(GameObject other)
    {
        if (other.tag != "Player") {
            return;
        }

        GameController.Instance.EnemyAggro += 1;
    }

    void OnLostTarget(GameObject other)
    {
        if (other.tag != "Player") {
            return;
        }

        GameController.Instance.EnemyAggro -= 1;
    }
}
