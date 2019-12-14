using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/**
 * TODO: Maybe refactor to support only one kind of event?
 */
public class TriggerZone : MonoBehaviour
{
    public event System.Action<GameObject> OnZoneEnter;
    public event System.Action<GameObject> OnZoneExit;

    [SerializeField] 
    private ZoneEvent onZoneEnter = null;
    [SerializeField] 
    private ZoneEvent onZoneExit = null;

    private CircleCollider2D collider;


    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();

        Assert.IsNotNull(collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        onZoneEnter.Invoke(other.transform.gameObject);

        if (OnZoneEnter != null) {
            OnZoneEnter(other.transform.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        onZoneExit.Invoke(other.transform.gameObject);

        if (OnZoneExit != null) {
            OnZoneExit(other.transform.gameObject);
        }
    }

    public float GetRadius()
    {
        return collider.radius;
    }
}

[System.Serializable]
public class ZoneEvent : UnityEvent<GameObject> {}