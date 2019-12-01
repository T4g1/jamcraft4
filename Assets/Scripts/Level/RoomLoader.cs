using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    public void LoadRoomContent()
    {
        Enemy[] enemies = GetComponentsInChildren<Enemy>(true);
        foreach(Enemy enemy in enemies) {
            enemy.gameObject.SetActive(true);
        }
    }
}
