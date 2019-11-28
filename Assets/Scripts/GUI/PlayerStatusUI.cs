using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField]
    private GameObject heartContainer = null;
    [SerializeField]
    private Text magazineClipText = null;


    void Start()
    {
        Assert.IsNotNull(heartContainer);
        Assert.IsNotNull(magazineClipText);

        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Weapon weapon = player.transform.GetComponentInChildren<Weapon>();

        player.OnHitPointsChanged += OnPlayerHitPointsChanged;
        weapon.OnMagazineClipChanged += OnPlayerMagazineClipChanged;
    }
    
    void OnPlayerHitPointsChanged(int value)
    {
        foreach (Button hearth in heartContainer.GetComponentsInChildren<Button>()) {
            hearth.interactable = value > 0;
            value -= 1;
        }
    }

    void OnPlayerMagazineClipChanged(uint value)
    {
        magazineClipText.text = value.ToString();
    }
}
