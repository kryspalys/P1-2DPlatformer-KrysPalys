using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach this to a box collider (Is Trigger) at the end of your level.
/// </summary>
public class LevelCompleteTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private bool levelFinished = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only trigger once, and only if the Player touches it
        if (!levelFinished && collision.CompareTag(playerTag))
        {
            levelFinished = true;

            // Tell the LevelTimer Singleton to stop counting and print the time
            if (LevelTimer.Instance != null)
            {
                LevelTimer.Instance.StopTimer();
            }

            // Reload the level after a 1.5 second delay
            Invoke("ResetLevel", 1.5f);
        }
    }

    private void ResetLevel()
    {
        // Grab the name of the current level and force it to reload from the beginning
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}