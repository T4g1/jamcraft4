using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabs = new List<GameObject>();
    private bool isSpawned = false;
    

    public void Spawn()
    {
        if (isSpawned) {
            return;
        }

        isSpawned = true;

        GameObject prefab = Utility.RandomElement(prefabs);
        if (prefab == null) {
            return;
        }

        GameController.Instance.Instantiate(
            prefab,
            gameObject.transform.position
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
