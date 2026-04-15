using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float currentSpeed;

    private Vector2 movementInput;
    private float moveDirection;

    void Start()
    {
        
    }

    void Update()
    {
        GetInput();
        transform.position += new Vector3(moveDirection < 0 ? movementInput.x : 0, moveDirection > 0 ? movementInput.y : 0) * speed * Time.deltaTime;
    }

    void GetInput()
    {
        movementInput = InputSystem.actions["move"].ReadValue<Vector2>();
        print(movementInput);
        moveDirection = InputSystem.actions["movedirection"].ReadValue<float>();
        print(moveDirection);
    }
}
