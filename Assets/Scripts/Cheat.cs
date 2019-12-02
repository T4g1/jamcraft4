using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    [SerializeField]
    private bool cheatEnabled = false;

    void Update()
    {
        if (!cheatEnabled) {
            return;
        }

        if (Input.GetButtonDown("Use")) {
            RandomizeWeapon();
        }
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
}
