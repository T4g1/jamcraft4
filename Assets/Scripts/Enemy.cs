using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private TriggerZone agroZone = null;

    [SerializeField]
    private TriggerZone lostZone = null;


    void Start()
    {
        agroZone.OnZoneEnter += OnGotTarget;
        lostZone.OnZoneExit += OnLostTarget;
    }

    void OnDestroy() 
    {
        agroZone.OnZoneEnter -= OnGotTarget;
        lostZone.OnZoneExit -= OnLostTarget;
    }

    void OnGotTarget()
    {
        Debug.Log("GOT TARGET");
    }

    void OnLostTarget()
    {
        Debug.Log("LOST TARGET");
    }
}
