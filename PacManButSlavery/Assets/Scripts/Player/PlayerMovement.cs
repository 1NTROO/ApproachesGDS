using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    private Vector3 currentSpeed;
    [SerializeField] private float maxSpeed = 10f;

    [Header("Stamina Settings")]
    [SerializeField] private float staminaConsumptionRate = 1f;
    [SerializeField] private float staminaConsumptionInterval = 1f;

    [Space(10)]
    [SerializeField] private float staminaRegenerationRate = 0.1f;
    [SerializeField] private float staminaRegenerationInterval = 1f;
    [SerializeField] private float staminaRegenerationDelay = 1.25f;
    private float staminaRegenerationDelayTimer = 0f;

    [Header("Chase Settings")]
    [SerializeField] private float graceTimer = 3f;
    private float graceTimerCurrent = 0f;

    private bool isMoving = false;
    private bool isConsumingStamina = false;
    private bool isRegeneratingStamina = false;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float moveDirection;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        GetInput();

        currentSpeed = new Vector3( moveDirection < 0 ? movementInput.x : 0, 
                                    moveDirection > 0 ? movementInput.y : 0) 
                                    * speed 
                                    * PlayerStatsManager.Instance.SpeedModifier
                                    * Time.deltaTime;

        currentSpeed = Vector3.ClampMagnitude(currentSpeed, maxSpeed * PlayerStatsManager.Instance.SpeedModifier);

        isMoving = PlayerMovingCheck();

        if (isMoving && !isConsumingStamina)
        {
            if (isRegeneratingStamina)
            {
                StopCoroutine("StaminaRegeneration");
                isRegeneratingStamina = false;
            }

            print("Started consuming stamina");

            isConsumingStamina = true;
            StartCoroutine("StaminaConsumption");
        }

        if (!isMoving && !isRegeneratingStamina)
        {
            print("Started regenerating stamina");
            isRegeneratingStamina = true;
            staminaRegenerationDelayTimer = 0f;
        }
        
        if (isRegeneratingStamina)
        {
            if (staminaRegenerationDelayTimer < staminaRegenerationDelay)
            {
                staminaRegenerationDelayTimer += Time.deltaTime;
                if (staminaRegenerationDelayTimer >= staminaRegenerationDelay)
                {
                    PlayerStamina stamina = GetComponent<PlayerStamina>();
                    stamina.RegenerateStamina(staminaRegenerationRate * 0.5f); // Apply half the regeneration rate during the delay period
                    staminaRegenerationDelayTimer = 2 * staminaRegenerationDelay; // Set timer to a value that indicates the delay period has passed
                    StartCoroutine("StaminaRegeneration");
                }
            }
        }
        
        rb.linearVelocity = currentSpeed;

        Vector3 normalizedVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 1f);

        animator.SetFloat("velocityX", normalizedVelocity.x);
        animator.SetFloat("velocityY", normalizedVelocity.y);

        animator.SetFloat("AnimSpeedModifier", PlayerStatsManager.Instance.SpeedModifier);

        if (!isMoving)
        {
            graceTimerCurrent += Time.deltaTime;
            if (graceTimerCurrent >= graceTimer)
            {
                GameManager.Instance.TransformPlayer = transform;
            }
        }
        else if (isMoving)
        {
            if (graceTimerCurrent > 0f)
            {
                graceTimerCurrent -= Time.deltaTime;
                if (graceTimerCurrent < 0f)
                {
                    graceTimerCurrent = 0f;
                }
                GameManager.Instance.TransformPlayer = null;
            }
        } 
    }

    void GetInput()
    {
        movementInput = InputSystem.actions["move"].ReadValue<Vector2>();
        moveDirection = InputSystem.actions["movedirection"].ReadValue<float>();
    }

    bool PlayerMovingCheck()
    {
        return movementInput.magnitude > 0.05f;
    }

    IEnumerator StaminaConsumption()
    {
        while (isConsumingStamina)
        {
            // print("Consuming stamina...");

            yield return new WaitForSeconds(staminaConsumptionInterval);

            PlayerStamina stamina = GetComponent<PlayerStamina>();
            stamina.ConsumeStamina(staminaConsumptionRate * (1 / PlayerStatsManager.Instance.StaminaConsumptionModifier)); // Consume stamina based on the defined rate

            isConsumingStamina = false; // Reset the flag to allow for the next consumption cycle
        }
    }

    IEnumerator StaminaRegeneration()
    {
        while (isRegeneratingStamina)
        {
            // print("Regenerating stamina...");

            yield return new WaitForSeconds(staminaRegenerationInterval);

            PlayerStamina stamina = GetComponent<PlayerStamina>();
            stamina.RegenerateStamina(staminaRegenerationRate); // Regenerate stamina based on the defined rate

            isRegeneratingStamina = false; // Reset the flag to allow for the next regeneration cycle
        }
    }

}
