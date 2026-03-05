using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PHYSICS APPROACH:
/// I chose to use Rigidbody2D with velocity manipulation. This approach leverages Unity's 
/// built-in physics engine to handle gravity and collisions natively, while allowing precise, 
/// code-driven control over horizontal momentum via custom acceleration and deceleration rates.
/// 
/// INPUT SYSTEM APPROACH:
/// I used polling in Update (ReadValue) for continuous horizontal movement to ensure smooth 
/// frame-by-frame updates. I used callback methods (performed/canceled) for discrete actions 
/// like Firing/Interacting and Jumping, as they only need to trigger once per button press.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxMoveSpeed = 5f; // Maximum horizontal speed the player can reach
    [SerializeField] private float acceleration = 20f; // Controls how fast the player reaches max speed
    [SerializeField] private float deceleration = 20f; // Controls how fast the player stops when input ceases

    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Reference to the player's Rigidbody2D component for physics manipulation
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer used for flipping the character graphic
    [SerializeField] private LayerMask interactableLayer; // Layer mask to filter which objects the player can interact with

    [Header("Interaction")]
    [SerializeField] private float rayLength = 15f; // Length of the raycast for detecting interactable objects
    [SerializeField] private GameObject bullet; // Prefab for the projectile the player can fire

    // Input System variables
    private ScriptingBinding PlayerInput; // Reference to the generated New Input System wrapper class
    private InputAction fireAction; // Reference to the fire/interact input action
    private InputAction moveAction; // Reference to the movement input action

    // State tracking variables
    private Vector2 moveInput; // Stores the current 2D vector input from the player's movement controls
    private Vector2 facingDirection = Vector2.right; // Default facing direction; stores the normalized direction the player is currently facing
    private Interactible currentInteractible; // Keeps track of the interactable object currently in range

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        PlayerInput = new ScriptingBinding();
    }

    private void OnEnable()
    {
        fireAction = PlayerInput.Player.Fire;
        fireAction.Enable();

        moveAction = PlayerInput.Player.Move;
        moveAction.Enable();
        fireAction.performed += Fire;
    }

    void OnDisable()
    {
        fireAction.performed -= Fire;
        moveAction.Disable();
        fireAction.Disable();
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        // Flip sprite based on direction
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
            facingDirection = new Vector2(moveInput.x, 0).normalized;
        }

        // Raycast logic for interactables
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, rayLength, interactableLayer);
        Debug.DrawRay(transform.position, facingDirection * rayLength, Color.red);

        if (hit.collider != null)
        {
            Interactible newTarget = null;
            if (hit.collider.GetComponent<Interactible>())
            {
                newTarget = hit.collider.GetComponent<Interactible>();
            }

            if (newTarget != currentInteractible)
            {
                if (currentInteractible != null)
                    currentInteractible.Highlight(false);

                currentInteractible = newTarget;
                if (currentInteractible != null)
                    currentInteractible.Highlight(true);
            }
        }
    }

    private void FixedUpdate()
    {
        // Calculate target speed based on input
        float targetSpeed = moveInput.x * maxMoveSpeed;

        // Determine whether to use acceleration (if moving) or deceleration (if stopping/changing direction)
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        // Move linear velocity towards the target speed to prevent instant start/stop
        float newXVelocity = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(newXVelocity, rb.linearVelocity.y);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentInteractible != null)
            {
                currentInteractible.Interact();
            }
        }
    }
}