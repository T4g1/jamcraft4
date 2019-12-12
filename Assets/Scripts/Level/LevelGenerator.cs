using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private CustomSlider loadingBar = null;
    [SerializeField]
    private Tilemap floorsTilemap = null;
    [SerializeField]
    private Tilemap wallsTilemap = null;

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
    public Transform Spawn {
        get { return spawn; }
        set {}
    }
    [SerializeField]
    private Portal portalIn = null;
    public Portal PortalIn {
        get { return portalIn; }
        set {}
    }
    private Portal portalOut = null;
    public Portal PortalOut {
        get { return portalOut; }
        set {}
    }

    private List<Room> rooms = new List<Room>();
    private List<Room> paths = new List<Room>();

    private GameObject startRoom = null;
    private GameObject endRoom = null;
    private GameObject dynamicHolder;


    void Start()
    {
        Assert.IsNotNull(floorsTilemap);
        Assert.IsNotNull(wallsTilemap);
        Assert.IsNotNull(startRoomContent);
        Assert.IsNotNull(endRoomContent);
        Assert.IsNotNull(roomCollider);
        Assert.IsNotNull(spawn);
        Assert.IsNotNull(portalIn);
        Assert.IsNotNull(loadingBar);

        Utility.AssertArrayNotNull<GameObject>(roomContents);

        dynamicHolder = GameController.Instance.dynamicHolder;

        Generate();
    }

    public void Generate()
    {
        StartCoroutine(_Generate());
    }

    IEnumerator _Generate()
    {
        loadingBar.Show();
        loadingBar.SetMaximalValue(9);
        loadingBar.SetCurrentValue(0);

        ClearEverything();

        AddRooms();
        loadingBar.SetCurrentValue(1);

        yield return new WaitForSeconds(generationTime);
        loadingBar.SetCurrentValue(2);

        FixRoomPositions();
        loadingBar.SetCurrentValue(3);

        yield return StartCoroutine(_FindPaths());
        loadingBar.SetCurrentValue(4);

        yield return StartCoroutine(_MakeCorridors());
        loadingBar.SetCurrentValue(5);

        yield return StartCoroutine(_FillRooms());
        loadingBar.SetCurrentValue(6);

        ClearRooms();
        loadingBar.SetCurrentValue(7);

        yield return StartCoroutine(_AddWalls());
        loadingBar.SetCurrentValue(8);

        yield return StartCoroutine(_TriggerSpawners());
        loadingBar.SetCurrentValue(9);

        GameController.Instance.OnLevelGenerated();
        
        loadingBar.Hide();
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
        wallsTilemap.ClearAllTiles();
        floorsTilemap.ClearAllTiles();
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

    void FixRoomPositions()
    {
        foreach (Room room in rooms) {
            room.DisableCollider();
        }
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
    IEnumerator _FindPaths()
    {
        Assert.IsTrue(rooms.Count > 0);

        ClearPath();

        List<Room> unprocessedRooms = new List<Room>(rooms);

        paths.Add(unprocessedRooms[0]);
        unprocessedRooms.Remove(unprocessedRooms[0]);

        while (unprocessedRooms.Count > 0) {
            yield return 0;

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

            //Debug.Log(current.gameObject.name + "->" + next.gameObject.name);

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

    IEnumerator _MakeCorridors()
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

            yield return 0;

            foreach (Room destination in origin.connectedTo) {
                Vector3 destinationPosition = new Vector3(
                    destination.GetPosition().x,
                    destination.GetPosition().y,
                    0.0f
                );
                
                MakeCorridor(originPosition, destinationPosition);
            }
        }
    }

    void MakeCorridor(Vector3 origin, Vector3 destination)
    {
        Vector3Int originCell = floorsTilemap.WorldToCell(origin);
        Vector3Int destinationCell = floorsTilemap.WorldToCell(destination);

        Vector3Int delta = destinationCell - originCell;
        delta.x = System.Math.Sign(delta.x);
        delta.y = System.Math.Sign(delta.y);

        if (delta.x == 0) {
            delta.x = (int) System.Math.Pow(-1, Random.Range(0, 1));
        }
        if (delta.y == 0) {
            delta.y = (int) System.Math.Pow(-1, Random.Range(0, 1));
        }

        Vector3Int offset = new Vector3Int(0, 0, 0) * delta;

        for (
            int x = originCell.x - offset.x; 
            x != destinationCell.x + offset.x; 
            x += delta.x
        ) {
            PlaceCorridorSection(
                new Vector3Int(x, originCell.y, 0),
                new Vector3Int(0, delta.y, 0)
            );
        }

        for (
            int y = originCell.y - offset.y; 
            y != destinationCell.y + offset.y; 
            y += delta.y
        ) {
            PlaceCorridorSection(
                new Vector3Int(destinationCell.x, y, 0), 
                new Vector3Int(delta.x, 0, 0)
            );
        }
    }

    void PlaceCorridorSection(
        Vector3Int position, Vector3Int delta)
    {
        floorsTilemap.SetTile(position, Utility.GetFloor());
        floorsTilemap.SetTile(position + delta, Utility.GetFloor());
    }

    /**
     * Add wals to every null tile adjacent to something that isnt a wall
     */
    IEnumerator _AddWalls()
    {
        floorsTilemap.CompressBounds();
        wallsTilemap.CompressBounds();
        
        BoundsInt bounds = floorsTilemap.cellBounds;
        TileBase[] allFloorsTiles = floorsTilemap.GetTilesBlock(bounds);
        TileBase[] allWallsTiles = wallsTilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++) {
            yield return 0;

            for (int y = 0; y < bounds.size.y; y++) {

                TileBase wallTile = allWallsTiles[x + y * bounds.size.x];
                TileBase floorTile = allFloorsTiles[x + y * bounds.size.x];
                if (wallTile == Utility.GetWall() || floorTile == null) {
                    continue;
                }
                
                Vector3Int[] neighbours = {
                    new Vector3Int(x - 1, y, 0),
                    new Vector3Int(x + 1, y, 0),
                    new Vector3Int(x, y - 1, 0),
                    new Vector3Int(x, y + 1, 0),
                    new Vector3Int(x - 1, y - 1, 0),
                    new Vector3Int(x + 1, y + 1, 0),
                    new Vector3Int(x + 1, y - 1, 0),
                    new Vector3Int(x - 1, y + 1, 0)
                };

                foreach (Vector3Int neighbour in neighbours) {
                    int index = neighbour.x + neighbour.y * bounds.size.x;
                    if (neighbour.x < 0 || 
                        neighbour.y < 0 || 
                        neighbour.x >= bounds.size.x || 
                        neighbour.y >= bounds.size.y ||
                        allFloorsTiles[index] == null
                    ) {
                        wallsTilemap.SetTile(
                            bounds.position + neighbour, 
                            Utility.GetWall()
                        );
                    }
                }
            }
        }
    }

    /**
     * Set tiles for every rooms
     */
    IEnumerator _FillRooms()
    {
        // Set destination of entry portal
        portalIn.SetDestination(startRoom.GetComponent<Room>().GetPosition());

        foreach (Room room in rooms) {
            yield return 0;

            room.GenerateFloors(floorsTilemap);
            room.GenerateWalls(wallsTilemap);

            yield return 0;

            GameObject content = room.GenerateContent(dynamicHolder);

            // Check for exit portal
            if (content.transform.Find("Portal") == null) {
                continue;
            }

            portalOut = content.transform.GetComponentInChildren<Portal>();
            portalOut.SetDestination(spawn.position);
            portalOut.SetLevelEnd(true);
        }
    }
    
    IEnumerator _TriggerSpawners()
    {
        Spawner[] spawners = 
            dynamicHolder.GetComponentsInChildren<Spawner>(true);
        foreach (Spawner spawner in spawners) {
            yield return 0;

            spawner.Spawn();
        }
    }
}
