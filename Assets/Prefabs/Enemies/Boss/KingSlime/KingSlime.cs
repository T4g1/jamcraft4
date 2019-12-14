using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class KingSlime : Slime
{
    [SerializeField]
    private Portal endPortal = null;
    [SerializeField]
    private GameObject subSlime = null;

    public void GotDamaged()
    {
        // TODO: Random spawn offset 
        // Spawn a subslime on damage
        GameObject spawnedSlime = GameController.Instance.Instantiate(
            subSlime,
            gameObject.transform.position
        );
    }

    void Awake()
    {
        Assert.IsNotNull(subSlime);

        OnTakeDamage += GotDamaged;
    }
    
    void OnDestroy() {
        OnTakeDamage -= GotDamaged;
    }

    public override void Die()
    {
        if (endPortal != null) {
            endPortal.Activate();
        }

        base.Die();
    }
}
