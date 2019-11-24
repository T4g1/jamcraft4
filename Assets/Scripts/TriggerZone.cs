using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public event System.Action OnZoneEnter;
    public event System.Action OnZoneExit;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (OnZoneEnter != null) {
            OnZoneEnter();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (OnZoneExit != null) {
            OnZoneExit();
        }
    }
}
