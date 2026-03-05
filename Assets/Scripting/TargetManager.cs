using UnityEngine;

/// <summary>
/// A persistent Singleton manager that tracks how many targets remain active in the scene.
/// </summary>
public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; } // Singleton instance accessible globally

    private int targetCount = 0; // Tracks the total number of active targets

    private void Awake()
    {
        // Singleton pattern enforcement
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Decrements the active target count. Called by targets when their health reaches zero.
    /// </summary>
    public void NotifyTargetDestroyed()
    {
        targetCount--;
        Debug.Log("Target destroyed. Remaining targets: " + targetCount);
    }
}