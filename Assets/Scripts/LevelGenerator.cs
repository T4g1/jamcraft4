using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap = null;

    [SerializeField]
    private uint generationTime = 5;

    [SerializeField]
    private uint roomCount = 30;

    [SerializeField]
    private float offsetRange = 10.0f;

    [SerializeField]
    private GameObject roomCollider = null;
    [SerializeField]
    private List<GameObject> roomContents = new List<GameObject>();
    [SerializeField]
    private GameObject startRoomContent = null;
    [SerializeField]
    private GameObject endRoomContent = null;
    [SerializeField]
    private Transform spawn = null;
    [SerializeField]
    private Portal portalIn = null;

    private List<GameObject> rooms = new List<GameObject>();
    private List<Room> path = new List<Room>();

    private GameObject startRoom = null;
    private GameObject endRoom = null;
    private GameObject dynamicHolder;
    private Portal portalOut = null;


    void Start()
    {
        Assert.IsNotNull(tilemap);
        Assert.IsNotNull(startRoomContent);
        Assert.IsNotNull(endRoomContent);
        Assert.IsNotNull(roomCollider);
        Assert.IsNotNull(spawn);
        Assert.IsNotNull(portalIn);

        Utility.AssertArrayNotNull<GameObject>(roomContents);

        dynamicHolder = GameController.Instance.dynamicHolder;

        Generate();
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Generate();
        }
        if (path != null)
        {
            if (path.Count != 0)
            {
                DrawPath();
            }
        }

    }

    public void Generate()
    {
        StartCoroutine(_Generate());
    }

    IEnumerator _Generate()
    {
        ClearEverything();
        AddRooms();


        yield return new WaitForSeconds(generationTime);

        path = FindPath(rooms);


        // FillRooms();
        // ClearRooms();

        // portalIn.Activate();

        // TODO: Should Happen when Boss Dies!
        // portalOut.Activate();
    }

    /**
     * Remove any tiles, rooms
     */
    public void ClearEverything()
    {
        ClearTilemap();
        ClearRooms();
        ClearDynamic();
        ClearPath();
    }
    void ClearPath()
    {
        path = null;
    }

    void ClearTilemap()
    {
        tilemap.ClearAllTiles();
    }

    void ClearRooms()
    {
        foreach (GameObject room in rooms)
        {
            DestroyImmediate(room);
        }

        rooms.Clear();
    }

    void ClearDynamic()
    {
        foreach (Transform child in dynamicHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /**
     * Add rooms
     */
    void AddRooms()
    {
        startRoom = AddRoom(startRoomContent, "start");

        for (uint i = 0; i < roomCount; i++)
        {
            int index = Random.Range(0, roomContents.Count);
            AddRoom(roomContents[index], i.ToString());
        }
        endRoom = AddRoom(endRoomContent, "end");
    }


    GameObject AddRoom(GameObject content, string i)
    {
        int offSet = (int)offsetRange;
        if (content == endRoomContent)
        {
            offSet = (int)offsetRange + 20;
        }
        GameObject roomObject = Instantiate(roomCollider);
        roomObject.transform.parent = dynamicHolder.transform;
        roomObject.transform.position = new Vector3(
            Random.Range(-offSet, offSet),
            Random.Range(-offSet, offSet),
            0.0f
        );

        Room room = roomObject.GetComponent<Room>();
        room.SetContent(content);

        rooms.Add(roomObject);
        roomObject.name = "Room " + i;

        return roomObject;
    }

    List<Room> FindPath(List<GameObject> roomList)
    {
        List<Room> pathList = new List<Room>();
        // Prim's Algorithm
        pathList.Add(roomList[0].GetComponent<Room>());
        roomList.Remove(roomList[0]);

        while (roomList.Count != 0)
        {
            float min_dist = Mathf.Infinity;
            Room current = null;
            GameObject next = null;
            Room nextRoom = null;
            foreach (Room r in pathList)
            {
                Vector2 rPos = r.GetPosition();
                foreach (GameObject gameObject2 in roomList)
                {
                    Room r2 = gameObject2.GetComponent<Room>();
                    Vector2 r2Pos = r2.GetPosition();
                    float dist = Vector2.Distance(rPos, r2Pos);
                    if (dist < min_dist)
                    {
                        current = r;
                        nextRoom = r2;
                        next = gameObject2;
                        min_dist = dist;
                    }
                }
            }
            Debug.Log(current.name + "->" + nextRoom.name);

            current.connectedTo.Add(next);

            pathList.Add(nextRoom);
            roomList.Remove(next);
        }
        return pathList;
    }
    void DrawPath()
    {
        foreach (Room room in path)
        {
            if (room.connectedTo.Count != 0)
            {
                Vector3 fromPos = new Vector3(
                    room.GetPosition().x
                    , room.GetPosition().y
                    , -1);
                foreach (GameObject toObj in room.connectedTo)
                {
                    Room toRoom = toObj.GetComponent<Room>();
                    Vector3 toPos = new Vector3(toRoom.GetPosition().x,
                    toRoom.GetPosition().y,
                    -1);
                    Debug.DrawLine(fromPos, toPos, Color.green);

                }

            }
        }
    }

    /**
     * Set tiles for every rooms
     */
    void FillRooms()
    {
        // Set destination of entry portal
        portalIn.SetDestination(startRoom.GetComponent<Room>().GetPosition());

        foreach (GameObject roomObject in rooms)
        {
            Room room = roomObject.GetComponent<Room>();
            GameObject content = room.Generate(tilemap, dynamicHolder);

            // Check for exit portal
            if (content.transform.Find("Teleport") == null)
            {
                continue;
            }

            portalOut = content.transform.GetComponentInChildren<Portal>();
            portalOut.SetDestination(spawn.position);
            portalOut.SetLevelEnd(true);
        }
    }
}
