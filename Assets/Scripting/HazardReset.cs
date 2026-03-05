using UnityEngine;

/// <summary>
/// Attach this to any hazard object with a 2D Collider set to "Is Trigger".
/// Teleports the player to a designated spawn point and halts their momentum.
/// </summary>
public class HazardReset : MonoBehaviour
{
    [Header("Reset Settings")]
    [SerializeField] private Transform spawnPoint; // Drag an empty GameObject here to act as the respawn location
    [SerializeField] private string playerTag = "Player"; // Make sure your Player GameObject has this tag

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger has the Player tag
        if (collision.CompareTag(playerTag))
        {
            // Move the player's position back to the spawn point
            collision.transform.position = spawnPoint.position;

            // Halt their physics momentum so they don't spawn with falling velocity
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
            }
        }
    }
}