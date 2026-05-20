using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerStatsManager numericals;

    void Start()
    {
        numericals = PlayerStatsManager.Instance;
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

            if (numericals != null)
            {
                numericals.AddPoints(numericals.PointsPerPickup);
                CurrentLevelManager levelManager = FindAnyObjectByType<CurrentLevelManager>();
                if (levelManager != null)
                {
                    levelManager.CheckEndLevel();
                }
            }
            // Handle trigger with pickup (e.g., increase score, play sound, etc.)
        }
        if (other.gameObject.layer == 11) // Power Pickup
        {
            Debug.Log("Triggered with Power Pickup!");

            other.gameObject.SetActive(false);

            if (numericals != null)
            {
                numericals.AddMoney(numericals.MoneyPerPickup);
            }
            // Handle trigger with power pickup (e.g., increase score, play sound, etc.)
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
