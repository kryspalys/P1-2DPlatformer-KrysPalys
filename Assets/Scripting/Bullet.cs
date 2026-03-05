using UnityEngine;

/// <summary>
/// Handles the movement and collision logic for a fired projectile.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Velocity multiplier for the projectile
    [SerializeField] private float lifeTime = 2f; // Time in seconds before the projectile self-destructs

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has a Target component
        if (collision.TryGetComponent(out Target target))
        {
            target.SendMessage("TakeDamage", 1);
            Destroy(gameObject); // Destroy the bullet after it successfully hits something
        }
    }

    /// <summary>
    /// Called immediately after instantiation to give the bullet its momentum.
    /// </summary>
    public void Initialize(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * speed;
        }
    }
}