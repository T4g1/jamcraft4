using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Gen : MonoBehaviour
{
    public GameObject floor_placeholder;

    public int world_rows, world_cols;
    public int minRoomSize, maxRoomSize;

    private GameObject[,] world_positions;



    public void BSPSplit(Partition sectioned)
    {
        if (sectioned.isLeaf())
        {
            // if the sectioned is too big
            if (sectioned.rect.width > maxRoomSize || sectioned.rect.height > maxRoomSize|| UnityEngine.Random.Range(0.0f, 1.0f) > 0.25)
            {
                if (sectioned.Split(minRoomSize, maxRoomSize))
                {
                    BSPSplit(sectioned.left_child);
                    BSPSplit(sectioned.right_child);
                }
            }
        }
    }
    public void DrawRooms(Partition sectioned)
    {
        // This function will get a desired prefab and populate the room with Fun:)
        if (sectioned == null) return;
        if (sectioned.isLeaf()){
            for (int i = (int)sectioned.room.x; i < sectioned.room.xMax; i++){
                for (int j = (int)sectioned.room.y; j < sectioned.room.yMax; j++){
                    GameObject instance = Instantiate(floor_placeholder, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(transform);
                    world_positions[i, j] = instance;
                }
            }
        }
        else{
            // Follow the Tree!
            DrawRooms(sectioned.left_child);
            DrawRooms(sectioned.right_child);
        }
    }

    void Start()
    {
        Partition root = new Partition(new Rect(0, 0, world_rows, world_cols));
        BSPSplit(root);
        root.CreateRoom();
        world_positions = new GameObject[world_rows, world_cols];
        DrawRooms(root);
        // TODO: Select START AND END.
        //       Select a Path.
        //       Either Push the rooms together or make Corridors.
        //       Set up Rooms from simple PreFabs.
    }
    void Update(){
        // Press Space to Restart the Scene...
        if(Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex) ;
        }
    }
}
