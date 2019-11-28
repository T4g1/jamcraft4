using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Arrow : Bullet
{
    [SerializeField]
    private GameObject arrowBounceEffect = null;


    void Start()
    {
        Assert.IsNotNull(arrowBounceEffect);
    }

    public override void OnMiss()
    {
        Utility.Instantiate(arrowBounceEffect, transform.position);
    }
}
