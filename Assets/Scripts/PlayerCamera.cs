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

    [SerializeField]
    private float shakeDuration = 0.5f;
    [SerializeField]
    private float defaultShakeMagnitude = 0.2f;
    [SerializeField]
    private float intenseShakeMagnitude = 0.4f;
    [SerializeField]
    private float dampingSpeed = 1.0f;

    private float shakeTimer = 0.0f;
    private float shakeMagnitude = 0.4f;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        shakeMagnitude = defaultShakeMagnitude;

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

        if (shakeTimer > 0.0f) {
            transform.position += Random.insideUnitSphere * shakeMagnitude;

            shakeTimer -= Time.deltaTime * dampingSpeed;
        }
    }

    public void TriggerShake()
    {
        shakeMagnitude = defaultShakeMagnitude;
        shakeTimer = shakeDuration;
    }

    public void TriggerIntenseShake()
    {
        shakeMagnitude = intenseShakeMagnitude;
        shakeTimer = shakeDuration;
    }
}
