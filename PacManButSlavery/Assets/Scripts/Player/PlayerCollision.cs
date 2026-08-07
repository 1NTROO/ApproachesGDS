using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    PlayerStatsManager numericals;

    [SerializeField] private float staminaDamageTaken = 5f;
    [SerializeField] private float staminaDamageTime = 1f;
    private float staminaDamageTimer = 0f;
    private bool isTakingStaminaDamage = false;

    [Header("Audio")]
    [SerializeField] private AudioClip[] pickupSounds;
    [SerializeField] private AudioClip[] powerPickupSounds;

    void Start()
    {
        numericals = PlayerStatsManager.Instance;
    }

    void Update()
    {
        if (isTakingStaminaDamage)
        {
            TakeStaminaDamage();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8) // Pickups
        {
            // Debug.Log("Triggered with Pickup!");
            
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
            
            AudioManager.Instance.PlayRandomSound(pickupSounds, 1, 0.1f, 0.4f);
        }
        if (other.gameObject.layer == 11) // Power Pickup
        {
            // Debug.Log("Triggered with Power Pickup!");

            other.gameObject.SetActive(false);

            if (numericals != null)
            {
                numericals.AddMoney(numericals.MoneyPerPickup);
            }
            
            AudioManager.Instance.PlayRandomSound(powerPickupSounds, 1, 0.1f, 0.4f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10) // Enemies
        {
            if (collision.gameObject.GetComponent<EnemyMovement>() != null)
            {
                if (collision.gameObject.GetComponent<EnemyMovement>().CurrentState != EnemyMovement.EnemyState.Chasing)
                {
                    staminaDamageTaken *= 0.5f; // Reduce stamina damage by 50% if the enemy is not chasing
                }
            }
            if (!isTakingStaminaDamage)
            {
                numericals.TakeStaminaDamage(staminaDamageTaken);
                isTakingStaminaDamage = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10) // Enemies
        {
            isTakingStaminaDamage = false;
        }
    }

    void TakeStaminaDamage()
    {
        staminaDamageTimer += Time.deltaTime;

        if (staminaDamageTimer >= staminaDamageTime)
        {
            numericals.TakeStaminaDamage(staminaDamageTaken);
            staminaDamageTimer = 0f;
        }

        // Wait for the next frame before continuing the loop
        System.Threading.Thread.Yield();
    }
}
