using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Gen : MonoBehaviour
{
    public GameObject floorPlaceholder;

    public int worldRows, worldCols;
    public int minRoomSize, maxRoomSize;
    [HideInInspector]
    public List<Room> worldRooms;
    int roomID;
    private List<Vector2> allowedSizes = new List<Vector2>{
        new Vector2(4.0f,4.0f),
        new Vector2(2.0f,2.0f),
        new Vector2(4.0f,2.0f),
        new Vector2(2.0f,4.0f),
        new Vector2(6.0f,6.0f)
    };
    private Room start, end;


    public void BSPSplit(Partition sectioned)
    {
        if (sectioned.isLeaf())
        {
            // if the sectioned is too big we split it!
            Vector2 dimensions = new Vector2(sectioned.rect.width, sectioned.rect.height);
            if (!allowedSizes.Contains(dimensions))
            {
                if (sectioned.Split(minRoomSize, maxRoomSize, allowedSizes))
                {
                    BSPSplit(sectioned.left_child);
                    BSPSplit(sectioned.right_child);
                }
            }
        }
    }
    public void DrawRooms(Partition sectioned)
    {
        // This function will get a desired prefab to populate the room with Fun:)
        if (sectioned == null) return;
        if (sectioned.isLeaf())
        {
            sectioned.weight = sectioned.room.width / sectioned.room.height;
            if (allowedSizes.Contains(new Vector2(sectioned.room.width, sectioned.room.height)))
            {
                Color roomColor = new Color(0, 1f, 0);

                GameObject instance = Instantiate(floorPlaceholder, new Vector3(sectioned.room.x + sectioned.room.width / 2, sectioned.room.y + sectioned.room.height / 2, 0f), Quaternion.identity) as GameObject;
                instance.name = "Room" + roomID.ToString();
                // +" "+sectioned.room.width.ToString() + "x" + sectioned.room.height;
                roomID++;
                instance.GetComponent<SpriteRenderer>().color = roomColor;
                instance.transform.localScale = new Vector3(sectioned.room.width, sectioned.room.height);
                instance.transform.SetParent(transform);
                worldRooms.Add(new Room(instance, sectioned.rect, sectioned.room));
            }

            // In Case we need to draw Tiles :
            // for (int i = (int)sectioned.room.x; i < sectioned.room.xMax; i++)
            //     for (int j = (int)sectioned.room.y; j < sectioned.room.yMax; j++)
        }
        else
        {
            // Follow the Tree!
            DrawRooms(sectioned.left_child);
            DrawRooms(sectioned.right_child);
        }

    }

    void MoveRooms()
    {
        foreach (Room room in worldRooms)
        {
            room.instance.GetComponent<RoomMovement>().enabled = true;
        }
    }
    void StopMoveRooms()
    {
        foreach (Room room in worldRooms)
        {
            room.instance.GetComponent<RoomMovement>().enabled = false;
            room.instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        }
    }
    void GetStartEnd()
    {
        foreach (Room r in worldRooms)
        {
            if (new Vector2(r.room.width, r.room.height).Equals(allowedSizes[0]))
            {
                start = r;
                break;
            }
        }
        worldRooms.Reverse();
        foreach (Room r in worldRooms)
        {
            if (r == start) break;
            if (new Vector2(r.room.width, r.room.height).Equals(allowedSizes[0]))
            {
                end = r;
                break;
            }
        }
        worldRooms.Reverse();

        if (start == null || end == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        start.isStart = true;
        end.isEnd = true;
        start.MarkStartEnd();
        end.MarkStartEnd();
    }


    IEnumerator OneSecond()
    {
        yield return new WaitForSeconds(1);
        StopMoveRooms();
    }
    void Start()
    {
        roomID = 0;
        Partition root = new Partition(new Rect(0, 0, worldRows, worldCols));
        BSPSplit(root);
        root.CreateRoom(allowedSizes);
        worldRooms = new List<Room>();
        DrawRooms(root);
        MoveRooms();
        GetStartEnd();
        // StartCoroutine(OneSecond());


        // TODO: Select START AND END.
        //       Select a Path.
        //       Either Push the rooms together or make Corridors.
        //       Set up Rooms from simple PreFabs.
    }
    void Update()
    {
        // Press Space to Restart the Scene...
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
