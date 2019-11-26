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
    private GameObject roomCollider = null;
    [SerializeField]
    private List<GameObject> roomContents = new List<GameObject>();
    [SerializeField]
    private GameObject startRoomContent = null;
    [SerializeField]
    private GameObject endRoomContent = null;
    [SerializeField]
    private GameObject craftRoomContent = null;
    [SerializeField]
    private GameObject playerPrefab = null;

    private List<GameObject> rooms = new List<GameObject>();
    private GameObject startRoom = null;
    private GameObject endRoom = null;

    [SerializeField]
    private GameObject craftRoom = null;

        [SerializeField]
    private GameObject teleportIn = null;

    private GameObject teleportOut=null;



    void Start()
    {
        Assert.IsNotNull(dynamicHolder);
        Assert.IsNotNull(tilemap);
        Assert.IsNotNull(startRoomContent);
        Assert.IsNotNull(endRoomContent);
        Assert.IsNotNull(playerPrefab);
        Assert.IsNotNull(roomCollider);
        Assert.IsNotNull(craftRoomContent);
        Assert.IsNotNull(teleportIn);
        Assert.IsTrue(roomContents.Count > 0);
        foreach (GameObject roomContent in roomContents) {
            Assert.IsNotNull(roomContent);
        }

        SpawnPlayer();
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

        ActivatePortal(teleportIn);
        ActivatePortal(teleportOut); //Should Happen when Boss Dies!
        

        
    }

    /**
     * Remove any tiles, rooms
     */
    void ClearEverything()
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
        foreach(GameObject room in rooms) {
            DestroyImmediate(room);
        }

        rooms.Clear();
    }

    void ClearDynamic()
    {
        foreach(Transform child in dynamicHolder.transform) {
            if(child.tag!="Player") DestroyImmediate(child.gameObject);
        }
    }

    void SetPortalDestination(GameObject portal,Vector2 destination){
        portal.GetComponent<Teleport>().destinationPosition= destination;
    }
    // Activates Portal.
    void ActivatePortal(GameObject portal){
        portal.SetActive(true);
    }


    /**
     * Add rooms
     */
    void AddRooms()
    {
        startRoom = AddRoom(startRoomContent);
        endRoom = AddRoom(endRoomContent);

        for(uint i=0; i<roomCount; i++) {
            int index = Random.Range(0, roomContents.Count);
            AddRoom(roomContents[index]);
        }
        
        SetPortalDestination(teleportIn,startRoom.GetComponent<Room>().GetPosition());
        teleportOut=endRoomContent.transform.Find("Content").Find("Teleport").gameObject;
        teleportOut.GetComponent<Teleport>().levelGenerator=this;

        SetPortalDestination(teleportOut,craftRoom.GetComponent<Room>().GetPosition());

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
        foreach(GameObject roomObject in rooms) {
            Room room = roomObject.GetComponent<Room>();
            room.Generate(tilemap, dynamicHolder);
        }
    }

    /**
     * Adds player
     */
    void SpawnPlayer()
    {
        Room room = craftRoom.GetComponent<Room>();

        GameObject player = Instantiate(playerPrefab);
        player.transform.parent = dynamicHolder.transform;
        player.transform.position = room.GetPosition();
    }
}
