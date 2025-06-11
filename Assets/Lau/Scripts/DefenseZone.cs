using UnityEngine;

public class DefendZone : MonoBehaviour
{
    [Tooltip("Expected incoming attack direction in local space (e.g., forward)")]
    public Vector3 expectedAttackDirection = Vector3.forward;

    [Tooltip("Allowed angular tolerance for blocking (in degrees)")]
    [Range(0f, 180f)]
    public float angleTolerance = 30f;

    /// <summary>
    /// Check if the given incoming attack direction (world-space) is blockable.
    /// </summary>
    /// <param name="incomingAttackDirection">The direction the attack is coming from, in world space.</param>
    public bool IsBlockSuccessful(Vector3 incomingAttackDirection)
    {
        Vector3 expectedDirWorld = transform.TransformDirection(expectedAttackDirection.normalized);
        float angle = Vector3.Angle(incomingAttackDirection.normalized, expectedDirWorld);
        return angle <= angleTolerance;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 worldExpectedDir = transform.TransformDirection(expectedAttackDirection.normalized);
        Vector3 origin = transform.position;

        // Draw expected attack direction
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, origin + worldExpectedDir * 2f);
        Gizmos.DrawSphere(origin + worldExpectedDir * 2f, 0.05f);

        // Draw cone representing the angle tolerance
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.2f);
        UnityEditor.Handles.color = Gizmos.color;
        UnityEditor.Handles.DrawSolidArc(origin, Vector3.up, Quaternion.Euler(0, -angleTolerance, 0) * worldExpectedDir, angleTolerance * 2f, 1.5f);
    }
#endif
}