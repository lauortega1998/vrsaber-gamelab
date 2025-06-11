using UnityEngine;

public class StrikeZone : MonoBehaviour
{
    public Vector3 requiredDirection = Vector3.right; // Local-space strike direction (e.g., right)
    public float angleTolerance = 30f;

    public bool IsCorrectStrike(Vector3 weaponVelocityWorld)
    {
        Vector3 weaponDir = weaponVelocityWorld.normalized;
        Vector3 requiredDir = transform.TransformDirection(requiredDirection.normalized);
        Vector3 oppositeDir = -requiredDir;

        float angleToRequired = Vector3.Angle(weaponDir, requiredDir);
        float angleToOpposite = Vector3.Angle(weaponDir, oppositeDir);

        return angleToRequired <= angleTolerance || angleToOpposite <= angleTolerance;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 worldDir = transform.TransformDirection(requiredDirection.normalized);
        Gizmos.DrawLine(transform.position, transform.position + worldDir * 1.5f);
        Gizmos.DrawSphere(transform.position + worldDir * 1.5f, 0.05f);

        // Show opposite direction as well
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position - worldDir * 1.5f);
        Gizmos.DrawSphere(transform.position - worldDir * 1.5f, 0.05f);
    }
#endif
}