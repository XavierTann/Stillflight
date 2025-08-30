using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerKeyboardMovement : MonoBehaviour
{
    public float speed = 2.0f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    // Input variables
    private Vector2 moveInput;
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        // Enable the controls when the object is enabled
        controls.Enable();
    }

    void OnDisable()
    {
        // Disable the controls when the object is disabled
        controls.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Bind the input actions
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void Update()
    {
        // Get the movement input (WASD / Arrow keys)
        float x = moveInput.x;
        float z = moveInput.y;

        // Move relative to player orientation
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
