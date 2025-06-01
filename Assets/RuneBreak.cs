using UnityEngine;

public class RuneTriggerRelay : MonoBehaviour
{
    public NewRuneScript runeManager;
    public float speedThreshold = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (runeManager == null) return;

        if (other.CompareTag("Weapon") && other.attachedRigidbody != null)
        {
            float impactSpeed = other.attachedRigidbody.linearVelocity.magnitude;
            if (impactSpeed >= speedThreshold)
            {
                runeManager.BreakRune();
            }
        }
    }
}
