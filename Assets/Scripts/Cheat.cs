using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Cheat : MonoBehaviour
{
    [SerializeField]
    private GameObject customSpawn = null;
    [SerializeField]
    private bool cheatEnabled = false;

    void Start()
    {
        if (cheatEnabled) {
            Debug.Log("WARNING: CHEAT MODE ACTIVATED, DEACTIVATE IT FOR PROD");
        } else {
            return;
        }
        
        Assert.IsNotNull(customSpawn);
    }

    void Update()
    {
        if (!cheatEnabled) {
            return;
        }

        /*if (Input.GetButtonDown("Use")) {
            RandomizeWeapon();
        }*/
    }

    /**
     * Regenerate each parts of the current Weapon
     */
    void RandomizeWeapon()
    {
        Weapon weapon = Utility.GetWeapon();
        for (int i = 1; i <= WeaponPart.GetPartTypeCount(); i++) {
            PartType partType = (PartType) i;
            WeaponPart part = GameController.Instance.GenerateRandomWeaponPart(
                partType
            );

            weapon.SetPart(part);
        }
    }

    void HurtPlayer()
    {
        Utility.GetPlayer().TakeDamage(1);
    }

    public void TeleportToDebugRoom()
    {
        if (!cheatEnabled) {
            return;
        }

        Utility.GetPlayer().transform.position = customSpawn.transform.position;
    }
}
