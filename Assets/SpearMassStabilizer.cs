using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpearAutoStabilizer : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Stabilization Settings")]
    public float stabilizationStrength = 5f; // How strongly it resists tipping over

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb != null && rb.linearVelocity.magnitude < 1f) // Only when spear is slow
        {
            Vector3 spearUp = transform.up;
            Vector3 worldUp = Vector3.up;

            // Calculate torque needed to align spear "up" with world up
            Vector3 torque = Vector3.Cross(spearUp, worldUp) * stabilizationStrength;

            // Apply stabilization torque
            rb.AddTorque(torque);
        }
    }
}
