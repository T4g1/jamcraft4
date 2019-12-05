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
    private bool isActive = false;
    
    [SerializeField]
    private bool defaultActive = true;
    [SerializeField]
    private Vector3 destination = Vector3.zero;
    [SerializeField]
    private Animator portalSprite = null;


    void Start()
    {
        Assert.IsNotNull(portalSprite);

        if (defaultActive) {
            Activate();
        }
        else {
            Deactivate();
        }
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!isActive) {
            return;
        }

        GameObject collider = other.gameObject;
        if (collider.tag == "Player") {
            StartCoroutine(_Teleport(collider));
        }
    }

    IEnumerator _Teleport(GameObject player)
    {
        yield return new WaitForSeconds(teleportTime);

        Deactivate();
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
        isActive = true;
        portalSprite.Play("opening");
    }

    public void Deactivate()
    {
        isActive = false;
        portalSprite.Play("closing");
    }
}