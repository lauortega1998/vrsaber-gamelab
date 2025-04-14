using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpearFlightStabilizer : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Stabilization Settings")]
    public float rotationSpeed = 5f; // Higher = faster rotation toward velocity

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > 0.5f)
        {
            // Calculate the target rotation based on current velocity
            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity, Vector3.up);

            // Smoothly rotate the spear toward the target direction
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}
