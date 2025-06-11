using UnityEngine;

public class RuneTriggerRelay : MonoBehaviour
{
    public NewRuneScript runeManager;
    public float speedThreshold = 2f;
    public GameObject sparkVFXPrefab;
    public float sparkCooldown = 1.5f;

    private float lastSparkTime = -Mathf.Infinity;

    private Vector3 lastWeaponPosition;
    private float lastVelocity = 0f;

    void Update()
    {
        // Only track weapon velocity when something is touching the rune
        // Optional: you can optimize this later if needed
    }

    private void OnTriggerEnter(Collider other)
    {
        if (runeManager == null || RuneSmashManager.runeAlreadySmashed) return;

        if (other.CompareTag("Weapon"))
        {
            float estimatedVelocity = EstimateWeaponVelocity(other);

            Debug.Log($"[RuneTriggerRelay] Estimated impact velocity: {estimatedVelocity:F2}");

            if (estimatedVelocity >= speedThreshold)
            {
                runeManager.BreakRune();
            }
            else
            {
                TryShowSpark(other);
            }
        }
    }

    private float EstimateWeaponVelocity(Collider weaponCollider)
    {
        // Estimate velocity based on position delta over time
        Vector3 currentPosition = weaponCollider.transform.position;
        float velocity = 0f;

        if (lastWeaponPosition != Vector3.zero)
        {
            velocity = (currentPosition - lastWeaponPosition).magnitude / Time.deltaTime;
        }

        lastWeaponPosition = currentPosition;
        return velocity;
    }

    private void TryShowSpark(Collider other)
    {
        if (Time.time - lastSparkTime < sparkCooldown) return;

        lastSparkTime = Time.time;

        if (sparkVFXPrefab != null)
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Instantiate(sparkVFXPrefab, contactPoint, Quaternion.identity);
        }
    }
}

