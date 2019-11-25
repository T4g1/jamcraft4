using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public event System.Action<GameObject> OnZoneEnter;
    public event System.Action<GameObject> OnZoneExit;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (OnZoneEnter != null) {
            OnZoneEnter(other.transform.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (OnZoneExit != null) {
            OnZoneExit(other.transform.gameObject);
        }
    }
}
