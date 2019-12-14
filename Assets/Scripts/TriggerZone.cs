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

    private CircleCollider2D circleCollider;


    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
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

    /**
     * Works only if the collider used is a CircleCollider2D
     */
    public float GetRadius()
    {
        if (circleCollider != null) {
            return circleCollider.radius;
        }

        return 0.0f;
    }
}

[System.Serializable]
public class ZoneEvent : UnityEvent<GameObject> {}