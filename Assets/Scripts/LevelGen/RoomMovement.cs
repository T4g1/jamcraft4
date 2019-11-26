using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomMovement : MonoBehaviour
{
    private Room room;
    private List<Room> placedRooms = new List<Room>();
    private Vector3 centroid;

    private bool canMove;
    void Start()
    {
        Level_Gen generationScript = gameObject.transform.parent.GetComponent<Level_Gen>();
        placedRooms = generationScript.worldRooms;

        centroid = new Vector3(generationScript.worldRows / 2, generationScript.worldRows / 2, 0);
        canMove = true;
        FindRoom();





    }
    void Update()
    {
        if (canMove)
        {
            float speed = 0.11f;
            // Vertical or Horizontal Movement.
            Vector3 distance = (centroid - gameObject.transform.position);
            if (Mathf.Max(Mathf.Abs(distance.x), Mathf.Abs(distance.y)) == Mathf.Abs(distance.x))
            {
                gameObject.transform.position += new Vector3(distance.x / Mathf.Abs(distance.x), 0f, 0f).normalized * speed;
            }
            else
            {
                gameObject.transform.position += new Vector3(0f, distance.y / Mathf.Abs(distance.y), 0f).normalized * speed;
            }
            if (distance.Equals(new Vector3(0, 0, 0)))
            {
                centroid += new Vector3(-1f, 0, 0);
                gameObject.transform.position += new Vector3(-1f, 0, 0).normalized * speed;

            };

        }
    }
    void FindRoom()
    {
        foreach (Room r in placedRooms)
        {
            if (r.instance.name == gameObject.name)
            {
                room = r;
                break;
            }
        }
    }
     void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        canMove = false;
        gameObject.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeAll;
        // Somewhy the Rooms are colliding. Check Room Prefab!
    }

}
