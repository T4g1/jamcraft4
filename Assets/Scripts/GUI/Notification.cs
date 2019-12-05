using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField]
    private string textContent = "";


    void Start()
    {
        GetComponentInChildren<Tooltip>().TextContent = textContent;
    }

    public void Show()
    {
        GetComponent<Animator>().enabled = true;
    }

    public void OnNitificationDisapeared()
    {
        Destroy(gameObject);
    }
}
