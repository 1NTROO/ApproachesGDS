using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector3 currentSpeed;
    [SerializeField] private float maxSpeed = 10f;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetInput();

        currentSpeed = new Vector3( moveDirection < 0 ? movementInput.x : 0, 
                                    moveDirection > 0 ? movementInput.y : 0) 
                                    * speed 
                                    * Time.deltaTime;

        currentSpeed = Vector3.ClampMagnitude(currentSpeed, maxSpeed); 
        
        rb.linearVelocity = currentSpeed;
    }

    void GetInput()
    {
        movementInput = InputSystem.actions["move"].ReadValue<Vector2>();
        moveDirection = InputSystem.actions["movedirection"].ReadValue<float>();
    }
}
