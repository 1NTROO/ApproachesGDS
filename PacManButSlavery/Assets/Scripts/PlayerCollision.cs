using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8) // Pickups
        {
            Debug.Log("Triggered with Pickup!");
            other.gameObject.SetActive(false);
            // Handle trigger with pickup (e.g., increase score, play sound, etc.)
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10) // Enemies
        {
            Debug.Log("Collided with Enemy!");
            // Handle collision with enemy (e.g., reduce health, trigger game over, etc.)
        }
    }
}
