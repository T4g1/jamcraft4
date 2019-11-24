using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Gen : MonoBehaviour
{
public GameObject initial_room;
public GameObject final_room;
public GameObject[] Rooms;
public GameObject[] Corridors;

public int iterations=5;

private GameObject populating;

private List<GameObject> placedRooms = new List<GameObject>();

void Start()
{
    Instantiate(initial_room);
    populating=initial_room;
    SpawnRooms();
}
void SpawnRooms(){
    
    // while (iterations>0){
        List<GameObject> unconnected_exits = new List<GameObject>();
        List<GameObject> connected_exits = new List<GameObject>();
        foreach (Transform child in populating.transform)
          {
              
              if (child.tag == "Door")
                  unconnected_exits.Add(child.gameObject);
          } 
        



    // }
}

}
