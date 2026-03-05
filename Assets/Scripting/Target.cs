using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventInt : UnityEvent<int> { }

/// <summary>
/// Represents a destructible object that can take damage and notify the TargetManager upon death.
/// </summary>
public class Target : MonoBehaviour
{
    public UnityEventInt OnHealthChanged = new UnityEventInt(); // Event triggered whenever health values update

    private int health = 3; // The current health of the target
    private SpriteRenderer spriteRenderer; // Visual component of the target

    private void Awake()
    {
        if (!TryGetComponent(out spriteRenderer))
        {
            enabled = false;
        }
    }

    private void Start()
    {
        OnHealthChanged.Invoke(health);
    }

    /// <summary>
    /// Subtracts health from the target and handles death logic.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (health == 0) return;

        health -= amount;
        OnHealthChanged.Invoke(health);

        if (health <= 0)
        {
            spriteRenderer.color = Color.gray;
            TargetManager.Instance.NotifyTargetDestroyed();
            gameObject.SetActive(false);
        }
    }
}