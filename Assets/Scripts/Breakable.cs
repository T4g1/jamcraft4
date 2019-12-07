using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : Alive
{
    public override void Die()
    {
        Destroy(gameObject);
    }
}
