using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffect;
    
    private Vector3 size;
    private Vector3 startPosition;


    void Start()
    {
        startPosition = transform.position;
        size = GetComponent<SpriteRenderer>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Utility.GetMouseInScreenPosition();
        Vector3 mouseCorrected = new Vector3(
            mousePosition.x - Screen.width / 2,
            mousePosition.y - Screen.height / 2,
            0
        );
        
        Vector3 distance = mouseCorrected * parallaxEffect;

        transform.position = startPosition + distance;
    }
}
