using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.0f;

    private Player player;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        Vector3 worldPosition = Utility.GetMouseWorldPosition();
        Vector3 delta = player.transform.position - worldPosition;

        Vector3 cameraPosition = player.transform.position - delta / 2;
        cameraPosition.z = -10.0f;

        transform.position =  Vector3.Lerp(
            transform.position, 
            cameraPosition, 
            Time.deltaTime * moveSpeed
        );
    }
}
