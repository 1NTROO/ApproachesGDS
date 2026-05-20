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


    private bool isMoving = false;
    private bool isConsumingStamina = false;
    private bool isRegeneratingStamina = false;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GetInput();

        currentSpeed = new Vector3( moveDirection < 0 ? movementInput.x : 0, 
                                    moveDirection > 0 ? movementInput.y : 0) 
                                    * speed 
                                    * Time.deltaTime;

        currentSpeed = Vector3.ClampMagnitude(currentSpeed, maxSpeed);

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
            StartCoroutine("StaminaRegeneration");
        }
        
        rb.linearVelocity = currentSpeed;

        GameManager.Instance.TransformPlayer = transform.position;
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
            stamina.ConsumeStamina(staminaConsumptionRate); // Consume stamina based on the defined rate

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
