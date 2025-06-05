using UnityEngine;

public class RuneTriggerRelay : MonoBehaviour
{
    public NewRuneScript runeManager;
    public float speedThreshold = 2f;
    public GameObject sparkVFXPrefab;
    public float sparkCooldown = 1.5f; // Delay between spark spawns

    private float lastSparkTime = -Mathf.Infinity;

    private void OnTriggerEnter(Collider other)
    {
        if (runeManager == null || RuneSmashManager.runeAlreadySmashed) return;

        if (other.CompareTag("Weapon") && other.attachedRigidbody != null)
        {
            float impactSpeed = other.attachedRigidbody.linearVelocity.magnitude;

            if (impactSpeed >= speedThreshold)
            {
                runeManager.BreakRune();
            }
            else
            {
                TryShowSpark(other);
            }
        }
    }

    private void TryShowSpark(Collider other)
    {
        if (Time.time - lastSparkTime < sparkCooldown) return;

        lastSparkTime = Time.time;

        if (sparkVFXPrefab != null)
        {
            // Find point of contact using closest point between weapon and rune
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Instantiate(sparkVFXPrefab, contactPoint, Quaternion.identity);
        }
    }
}
