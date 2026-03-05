using UnityEngine;

/// <summary>
/// Tracks the time since the level loaded and prints the final time to the console.
/// </summary>
public class LevelTimer : MonoBehaviour
{
    public static LevelTimer Instance { get; private set; }

    private float currentTime = 0f;
    private bool isTiming = true; // Starts automatically when the scene loads

    private void Awake()
    {
        // Singleton setup so the Finish Line can easily find this script
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isTiming)
        {
            currentTime += Time.deltaTime; // Count up in seconds
        }
    }

    /// <summary>
    /// Halts the timer and logs the formatted completion time.
    /// </summary>
    public void StopTimer()
    {
        isTiming = false;

        // Formats the float to show exactly 2 decimal places (e.g., "45.23 seconds")
        Debug.Log($"<color=green>LEVEL COMPLETE! Final Time: {currentTime:F2} seconds.</color>");
    }
}