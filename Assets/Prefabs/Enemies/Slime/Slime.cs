using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public override void Attack()
    {
        Utility.GetPlayer().TakeDamage(1);
    }
}
