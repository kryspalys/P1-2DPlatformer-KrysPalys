using TMPro;
using UnityEngine;

/// <summary>
/// Handles behavior for objects the player can interact with in the environment.
/// </summary>
public class Interactible : MonoBehaviour
{
    [SerializeField] private LayerMask interactableMask; // Layer mask defining what can interact with this object
    [SerializeField] private Color highlightColor = Color.yellow; // The color the object changes to when in range

    private SpriteRenderer spriteRenderer; // Reference to the object's visual renderer
    private Color originalColor; // Stores the default color to revert back to
    private TextMeshPro interactionText; // Reference to the floating UI text prompt

    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out spriteRenderer);
        originalColor = GetComponent<SpriteRenderer>().color;

        interactionText = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    /// <summary>
    /// Toggles the visual highlight and UI prompt when the player is near.
    /// </summary>
    public void Highlight(bool isActive)
    {
        if (isActive)
        {
            spriteRenderer.color = highlightColor;
            interactionText.text = "Press E to interact";
        }
        else
        {
            spriteRenderer.color = originalColor;
            interactionText.text = "";
        }
    }

    /// <summary>
    /// Executes the core logic when the player actually triggers the interaction.
    /// </summary>
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        gameObject.SetActive(false);
    }
}