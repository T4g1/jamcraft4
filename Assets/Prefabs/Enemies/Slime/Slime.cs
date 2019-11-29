using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private float distanceToPlayer;
    private void Update() {
        distanceToPlayer=Vector3.Distance(gameObject.transform.position, new Vector3(0.0f,0.0f,0.0f));
    }
    override public void Attack(){
        Debug.Log("Player Attacked");
    }
   
}
