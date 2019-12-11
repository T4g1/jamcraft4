using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Alive : MonoBehaviour
{
    public event System.Action OnTakeDamage;
    public event System.Action OnDeath;
    public event System.Action<int> OnHitPointsChanged;
    
    [FMODUnity.EventRef]
    public string dieSFX = "";
    [FMODUnity.EventRef]
    public string hurtSFX = "";

    [SerializeField]
    protected int maxHitPoints = 1;
    protected int hitPoints = 0;
    public int HitPoints {
        get { return hitPoints; }
        set {
            bool wasAlive = IsAlive;
            hitPoints = value;

            if (OnHitPointsChanged != null) {
                OnHitPointsChanged(hitPoints);
            }

            // Just died
            if (wasAlive && !IsAlive) {
                Die();
            }
        }
    }

    protected virtual void Start()
    {
        Heal();
    }
    
    public bool IsAlive {
        get { return HitPoints > 0; }
        set {}
    }

    public virtual void TakeDamage(int amount)
    {
        HitPoints -= amount;
        
        if (hurtSFX != "") {
            Utility.PlaySFX(hurtSFX);
        }

        if (OnTakeDamage != null) {
            OnTakeDamage();
        }
    }

    public virtual void Die()
    {
        if (dieSFX != "") {
            Utility.PlaySFX(dieSFX);
        }

        if (OnDeath != null) {
            OnDeath();
        }
    }

    public virtual void Heal()
    {
        HitPoints = maxHitPoints;
    }
}
