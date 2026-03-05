using TMPro;
using UnityEngine;

/// <summary>
/// A simple UI component that updates a TextMeshPro field with current health values.
/// </summary>
public class HealthDisplay : MonoBehaviour
{
    private TextMeshPro healthText; // Reference to the text UI element

    void Awake()
    {
        TryGetComponent(out healthText);
    }

    /// <summary>
    /// Updates the string displayed on the UI.
    /// </summary>
    public void UpdateHealthDisplay(int currentHealth)
    {
        healthText.text = $"Health: {currentHealth}";
    }
}