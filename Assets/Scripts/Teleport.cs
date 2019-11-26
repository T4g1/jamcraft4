using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Teleport : MonoBehaviour
{
    [HideInInspector]
    public Vector2 destinationPosition;
    [SerializeField]
    private float teleportTime=0.5f;

    public LevelGenerator levelGenerator;

    [SerializeField]
    public bool reGenerates=false;
    
     private void OnTriggerEnter2D(Collider2D other) {
         if(other.gameObject.tag=="Player"){
             StartCoroutine(_Teleport(other.gameObject));
         }
    }
    IEnumerator _Teleport(GameObject player)
    {
        yield return new WaitForSeconds(teleportTime);

        player.transform.position= destinationPosition;
        if(reGenerates){
            levelGenerator.Generate();
        }
        gameObject.SetActive(false);
        
    }
}