using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightable : MonoBehaviour
{
    [SerializeField]
    private GameObject lightObject = null;
    [SerializeField]
    private bool triggerOnce = true;
    private bool triggered = false;


    public void OnTriggered()
    {
        if (triggerOnce && triggered) {
            return;
        }
        
        triggered = true;

        if (lightObject == null) {
            return;
        }
        
        GameController.Instance.Instantiate(
            lightObject,
            gameObject.transform.position
        );
    }
}
