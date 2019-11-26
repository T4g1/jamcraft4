using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject dynamicHolder = null;

    [SerializeField]
    private Tilemap tilemap = null;

    [SerializeField]
    private uint generationTime = 5;

    [SerializeField]
    private uint roomCount = 30;

    [SerializeField]
    private float offsetRange = 10.0f;

    [SerializeField]
    private List<GameObject> roomPrefabs = new List<GameObject>();
    [SerializeField]
    private GameObject startRoomPrefab = null;
    [SerializeField]
    private GameObject endRoomPrefab = null;

    private List<GameObject> rooms = new List<GameObject>();
    private GameObject startRoom = null;
    private GameObject endRoom = null;

    void Start()
    {
        Assert.IsNotNull(dynamicHolder);
        Assert.IsNotNull(tilemap);
        Assert.IsNotNull(startRoomPrefab);
        Assert.IsNotNull(endRoomPrefab);
        Assert.IsTrue(roomPrefabs.Count > 0);
        foreach (GameObject roomPrefab in roomPrefabs) {
            Assert.IsNotNull(roomPrefab);
        }

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
    }

    /**
     * Remove any tiles, rooms
     */
    void ClearEverything()
    {
        ClearTilemap();
        ClearRooms();
    }

    void ClearTilemap()
    {
        tilemap.ClearAllTiles();
    }

    void ClearRooms()
    {
        foreach(GameObject room in rooms) {
            DestroyImmediate(room);
        }

        rooms.Clear();
    }

    /**
     * Add rooms
     */
    void AddRooms()
    {
        startRoom = AddRoom(startRoomPrefab);
        endRoom = AddRoom(endRoomPrefab);

        for(uint i=0; i<roomCount; i++) {
            int index = Random.Range(0, roomPrefabs.Count);
            AddRoom(roomPrefabs[index]);
        }
    }

    GameObject AddRoom(GameObject prefab)
    {
        GameObject roomObject = Instantiate(prefab);
        roomObject.transform.parent = dynamicHolder.transform;
        roomObject.transform.position = new Vector3(
            Random.Range(-offsetRange, offsetRange),
            Random.Range(-offsetRange, offsetRange),
            0.0f
        );

        rooms.Add(roomObject);

        Room room = roomObject.GetComponent<Room>();
        room.Size = room.Size;

        return roomObject;
    }

    /**
     * Set tiles for every rooms
     */
    void FillRooms()
    {
        foreach(GameObject roomObject in rooms) {
            /*roomObject.transform.position = new Vector3(
                Mathf.Floor(roomObject.transform.position.x),
                Mathf.Floor(roomObject.transform.position.y),
                0.0f
            );*/
            Room room = roomObject.GetComponent<Room>();
            room.Generate(tilemap);
        }
    }
}
