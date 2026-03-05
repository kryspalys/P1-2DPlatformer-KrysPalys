using UnityEngine;

/// <summary>
/// Handles smooth 2D camera following with optional boundary clamping.
/// Utilizes Vector3.SmoothDamp to prevent instant snapping and Mathf.Clamp to keep the camera within the level bounds.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform m_FollowTarget; // The player or object the camera should track

    [SerializeField] private Vector3 m_Offset = new Vector3(0, 1, -10); // The positional offset to maintain from the target

    [Header("Boundaries")]
    [SerializeField] private bool useBound = true; // Toggle to enable or disable camera boundaries
    [SerializeField] private float minX = 0; // The furthest left the camera is allowed to travel
    [SerializeField] private float maxX = 0; // The furthest right the camera is allowed to travel
    [SerializeField] private float minY = 0; // The lowest the camera is allowed to travel
    [SerializeField] private float maxY = 0; // The highest the camera is allowed to travel

    private float smoothTime = 0.25f; // The approximate time it will take to reach the target
    private Vector3 velocity; // Reference variable used by SmoothDamp to calculate movement

    private void LateUpdate()
    {
        if (m_FollowTarget == null) return;

        // Determine where the camera wants to be
        Vector3 desiredTarget = m_FollowTarget.position + m_Offset;

        // Smoothly transition from the current position to the desired position
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredTarget, ref velocity, smoothTime);

        // Lock the camera within the defined coordinates if boundaries are enabled
        if (useBound)
        {
            smoothPosition.x = Mathf.Clamp(smoothPosition.x, minX, maxX);
            smoothPosition.y = Mathf.Clamp(smoothPosition.y, minY, maxY);
        }

        transform.position = smoothPosition;
    }
}