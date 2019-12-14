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
        // Spawn a subslime on damage
        float offset = 1.0f;
        GameObject spawnedSlime = GameController.Instance.Instantiate(
            subSlime,
            gameObject.transform.position + new Vector3(
                Random.Range(-offset, offset),
                Random.Range(-offset, offset),
                0.0f
            )
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
