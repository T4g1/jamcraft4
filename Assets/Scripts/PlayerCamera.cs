using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float margin = 100.0f;
    [SerializeField]
    private float moveSpeed = 2.0f;

    private Player player;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        // Confines the mouse in the screen coordinates
        Vector3 screenPosition = Input.mousePosition;
        if (screenPosition.x < margin) {
            screenPosition.x = margin;
        }
        if (screenPosition.y < margin) {
            screenPosition.y = margin;
        }
        if (screenPosition.x > Screen.width - margin) {
            screenPosition.x = Screen.width - margin;
        }
        if (screenPosition.y > Screen.height - margin) {
            screenPosition.y = Screen.height - margin;
        }

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

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
