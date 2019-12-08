using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : Alive
{
    [SerializeField]
    private GameObject brokenEffect = null;


    public override void Die()
    {
        if (brokenEffect != null) {
            GameController.Instance.Instantiate(
                brokenEffect,
                gameObject.transform.position
            );
        }

        Destroy(gameObject);
    }
}
