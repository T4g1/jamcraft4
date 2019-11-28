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

    private List<Room> rooms = new List<Room>();
    private List<Room> paths = new List<Room>();

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
        if (Input.GetButtonDown("Submit")) {
            Generate();
        }

        if (paths.Count > 0) {
            DrawPath();
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

        FindPaths();

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
        paths.Clear();
    }

    void ClearTilemap()
    {
        tilemap.ClearAllTiles();
    }

    void ClearRooms()
    {
        foreach (Room room in rooms) {
            DestroyImmediate(room.gameObject);
        }

        rooms.Clear();
    }

    void ClearDynamic()
    {
        foreach (Transform child in dynamicHolder.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    /**
     * Add rooms
     */
    void AddRooms()
    {
        startRoom = AddRoom(startRoomContent, "start");

        for (uint i = 0; i < roomCount; i++) {
            int index = Random.Range(0, roomContents.Count);
            AddRoom(roomContents[index], "Room " + i.ToString());
        }

        endRoom = AddRoom(endRoomContent, "end");
    }

    GameObject AddRoom(GameObject content, string name = "New Room")
    {
        float offset = offsetRange;
        if (content == endRoomContent) {
            offset = offsetRange + 20.0f;
        }

        GameObject roomObject = Instantiate(roomCollider);
        roomObject.name = name;
        roomObject.transform.parent = dynamicHolder.transform;
        roomObject.transform.position = new Vector3(
            Random.Range(-offset, offset),
            Random.Range(-offset, offset),
            0.0f
        );

        Room room = roomObject.GetComponent<Room>();
        room.SetContent(content);

        rooms.Add(room);

        return roomObject;
    }

    /** 
     * Prim's Algorithm
     */
    void FindPaths()
    {
        Assert.IsTrue(rooms.Count > 0);

        ClearPath();

        List<Room> unprocessedRooms = new List<Room>(rooms);

        paths.Add(unprocessedRooms[0]);
        unprocessedRooms.Remove(unprocessedRooms[0]);

        while (unprocessedRooms.Count > 0) {
            float minimalDistance = Mathf.Infinity;

            Room current = null;
            Room next = null;

            // Find the smallest distance between any two room
            foreach (Room room1 in paths) {
                Vector2 room1Position = room1.GetPosition();

                foreach (Room room2 in unprocessedRooms) {
                    float distance = Vector2.Distance(
                        room1Position, 
                        room2.GetPosition()
                    );

                    if (distance < minimalDistance) {
                        minimalDistance = distance;
                        current = room1;
                        next = room2;
                    }
                }
            }

            Debug.Log(current.gameObject.name + "->" + next.gameObject.name);

            current.connectedTo.Add(next);

            paths.Add(next);
            unprocessedRooms.Remove(next);
        }
    }

    void DrawPath()
    {
        foreach (Room origin in paths) {
            if (origin.connectedTo.Count <= 0) {
                continue;
            }

            Vector3 originPosition = new Vector3(
                origin.GetPosition().x,
                origin.GetPosition().y,
                0.0f
            );

            foreach (Room destination in origin.connectedTo) {
                Vector3 destinationPosition = new Vector3(
                    destination.GetPosition().x,
                    destination.GetPosition().y,
                    0.0f
                );

                Debug.DrawLine(
                    originPosition, 
                    destinationPosition, 
                    Color.green
                );
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

        foreach (Room room in rooms) {
            GameObject content = room.Generate(tilemap, dynamicHolder);

            // Check for exit portal
            if (content.transform.Find("Teleport") == null) {
                continue;
            }

            portalOut = content.transform.GetComponentInChildren<Portal>();
            portalOut.SetDestination(spawn.position);
            portalOut.SetLevelEnd(true);
        }
    }
}
