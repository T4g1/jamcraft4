using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private float teleportTime = 0.5f;

    private bool isLevelEnd = false;
    
    [SerializeField]
    private bool defaultActive = true;
    [SerializeField]
    private Vector3 destination = Vector3.zero;


    void Start()
    {
        if (defaultActive) {
            Activate();
        }
        else {
            Deactivate();
        }
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        GameObject collider = other.gameObject;
        if (collider.tag == "Player") {
            StartCoroutine(_Teleport(collider));
        }
    }

    IEnumerator _Teleport(GameObject player)
    {
        yield return new WaitForSeconds(teleportTime);

        gameObject.SetActive(false);
        player.transform.position = destination;

        if (isLevelEnd) {
            GameController.Instance.OnLevelEnds();
        }
    }

    public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
    }

    public void SetLevelEnd(bool value)
    {
        isLevelEnd = value;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}