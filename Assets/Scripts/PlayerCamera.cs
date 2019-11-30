using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.0f;

    private Player player;
    private float marginTop;
    private float marginBottom;
    private float marginLeft;
    private float marginRight;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        marginTop = Screen.height / 3;
        marginBottom = Screen.height / 3;
        marginLeft = Screen.width / 3;
        marginRight = Screen.width / 3;
    }

    void Update()
    {
        if (!player.InputEnabled()) {
            return;
        }
        
        // Confines the mouse in the screen coordinates
        Vector3 screenPosition = Input.mousePosition;
        if (screenPosition.x < marginRight) {
            screenPosition.x = marginRight;
        }
        if (screenPosition.y < marginTop) {
            screenPosition.y = marginTop;
        }
        if (screenPosition.x > Screen.width - marginLeft) {
            screenPosition.x = Screen.width - marginLeft;
        }
        if (screenPosition.y > Screen.height - marginBottom) {
            screenPosition.y = Screen.height - marginBottom;
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
