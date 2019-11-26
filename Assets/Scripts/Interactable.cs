using UnityEngine;

public class Interactable : MonoBehaviour 
{
    public float radius = 3.0f;

    void Awake()
    {
        UpdateRadius();
    }

    public virtual void Interact()
    {
        // To be overrided
    }

    void UpdateRadius()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = radius;
    }

    void OnDrawGizmosSelected() 
    {
        UpdateRadius();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (!player) {
            return;
        }

        Interact();
    }
}