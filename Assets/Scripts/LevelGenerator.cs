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

        FillRooms();
        ClearRooms();

        portalIn.Activate();

        // TODO: Should Happen when Boss Dies!
        portalOut.Activate();
    }

    /**
     * Remove any tiles, rooms
     */
    public void ClearEverything()
    {
        ClearTilemap();
        ClearRooms();
        ClearDynamic();
    }

    void ClearTilemap()
    {
        tilemap.ClearAllTiles();
    }

    void ClearRooms()
    {
        foreach (GameObject room in rooms) {
            DestroyImmediate(room);
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
        startRoom = AddRoom(startRoomContent);
        endRoom = AddRoom(endRoomContent);

        for (uint i = 0; i < roomCount; i++) {
            int index = Random.Range(0, roomContents.Count);
            AddRoom(roomContents[index]);
        }
    }

    GameObject AddRoom(GameObject content)
    {
        GameObject roomObject = Instantiate(roomCollider);
        roomObject.transform.parent = dynamicHolder.transform;
        roomObject.transform.position = new Vector3(
            Random.Range(-offsetRange, offsetRange),
            Random.Range(-offsetRange, offsetRange),
            0.0f
        );

        Room room = roomObject.GetComponent<Room>();
        room.SetContent(content);

        rooms.Add(roomObject);

        return roomObject;
    }

    /**
     * Set tiles for every rooms
     */
    void FillRooms()
    {
        // Set destination of entry portal
        portalIn.SetDestination(startRoom.GetComponent<Room>().GetPosition());

        foreach (GameObject roomObject in rooms) {
            Room room = roomObject.GetComponent<Room>();
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
