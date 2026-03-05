using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// GROUND DETECTION METHOD:
/// I used Physics2D.OverlapCircle at the character's base paired with a LayerMask. 
/// This method is highly reliable because it creates a defined collision area specifically 
/// filtered to only trigger on designated "Ground" layers, preventing false positives from walls or enemies.
/// 
/// ADVANCED JUMP TECHNIQUES:
/// I implemented BOTH Coyote Time and Jump Buffering. Coyote Time allows the player to jump briefly 
/// after walking off a ledge, while Jump Buffering registers a jump input slightly before landing, 
/// making the platforming feel highly responsive and forgiving.
/// </summary>
public class PlayerJump : MonoBehaviour
{
    // Component references
    ScriptingBinding inputActions; // Reference to the generated New Input System wrapper class
    Rigidbody2D rb; // Reference to the player's Rigidbody2D component

    [Header("Jump Settings")]
    public float jumpForce = 5f; // Adjust the jump force as needed
    [SerializeField] private float jumpCutMultiplier = 0.5f; // Multiplier to reduce jump height when jump is released early
    [SerializeField] private float fallMultiplier = 2.5f; // Multiplier to increase fall speed for better jump feel
    [SerializeField] private float lowJumpMultiplier = 2f; // Multiplier to increase fall speed when jump is released early for better jump feel
    [SerializeField] private float maxFallSpeed = -10f; // Maximum fall speed to prevent excessive falling

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer; // Layer to check for ground
    [SerializeField] private Transform groundCheck; // Empty GameObject positioned at the player's feet
    [SerializeField] private float groundCheckRadius = 0.2f; // Radius for ground check

    [Header("Advanced Jump Mechanics")]
    [SerializeField] private float coyoteTime = 0.2f; // Time after leaving the ground during which a jump can still be initiated
    [SerializeField] private float jumpBufferTime = 0.2f; // Time before landing during which a jump input can be buffered

    // Timers for advanced mechanics
    private float coyoteTimeCounter; // Actively tracks the remaining coyote time in seconds
    private float jumpBufferCounter; // Actively tracks the remaining jump buffer time in seconds

    // Input state flags
    private bool isJumpPressed = false; // Flag indicating the jump button was just pressed this frame
    private bool isJumpReleased = false; // Flag indicating the jump button was just released this frame
    private bool isJumpHeld = false; // Flag indicating the jump button is currently being held down
    private bool isJumping = false; // Flag indicating the player is currently in an active jump state

    void Awake()
    {
        inputActions = new ScriptingBinding();
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("Rigidbody2D component is missing on the player object. Disabling PlayerJump script.");
        }
    }

    void OnEnable()
    {
        inputActions.Player.Jump.Enable();
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Jump.canceled += OnJumpCanceled;
    }

    void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Jump.canceled -= OnJumpCanceled;
        inputActions.Player.Jump.Disable();
    }

    private void Update()
    {
        bool isGrounded = IsGrounded();

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time when grounded
            isJumping = false; // Reset jump input when grounded
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Decrease coyote time counter when not grounded
        }

        if (isJumpPressed)
        {
            jumpBufferCounter = jumpBufferTime; // Reset jump buffer counter when jump is pressed
            isJumpPressed = false; // Reset jump input after buffering
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime; // Decrease jump buffer counter when not pressed
        }

        // Jump Execution
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Adjust the jump force as needed
            isJumping = true; // Set jump input as active
            coyoteTimeCounter = 0f; // Reset coyote time counter after jumping
            jumpBufferCounter = 0f; // Reset jump buffer counter after jumping
        }

        // Variable Jump Height
        if (isJumpReleased && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier); // Reduce upward velocity for variable jump height
            isJumpReleased = false; // Reset jump release input
        }

        isJumpReleased = false; // Reset jump release input at the end of the frame to prevent unintended behavior
    }

    private void FixedUpdate()
    {
        // Apply fall multiplier for better jump feel
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0f && !isJumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Clamp fall speed to prevent excessive falling
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJumpPressed = true;
            isJumpHeld = true;
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            isJumpReleased = true;
            isJumpHeld = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            // Call our existing method to check the current ground state
            if (IsGrounded())
            {
                Gizmos.color = Color.green; // Touching the ground
            }
            else
            {
                Gizmos.color = Color.red; // In the air
            }

            // Draw the sphere using the selected color
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private bool IsGrounded()
    {
        // Check if the player is touching the ground using a circle overlap
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}