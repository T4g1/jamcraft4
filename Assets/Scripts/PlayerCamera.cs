using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerCamera : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private float margin = 100.0f;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
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

        transform.position = cameraPosition;
    }
}
