using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Hit Effect")]
    public GameObject hitEffectPrefab;

    [Header("Broken Version")]
    public GameObject brokenSkeletonPrefab;

    [Header("Death Effects")]
    public float knockbackForce = 5f;
    public float enemyDestroyDelay = 2f;

    public event Action OnDeath;

    public void Die(Transform weaponHitPoint)
    {
        // Disable the main collider
        GetComponent<Collider>().enabled = false;

        EnemyManager.Instance.UnregisterAttacker();
        GameObject enemyRoot = transform.root.gameObject;
        Debug.Log($"{enemyRoot.name} was killed!");
        KillCounter.Instance.AddKill();
        OnDeath?.Invoke();

        // Spawn hit effect
        if (hitEffectPrefab != null && weaponHitPoint != null)
        {
            Vector3 hitPoint = GetComponent<Collider>().ClosestPoint(weaponHitPoint.position);
            GameObject effect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // Instantiate broken version
        if (brokenSkeletonPrefab != null)
        {
            GameObject brokenInstance = Instantiate(brokenSkeletonPrefab, enemyRoot.transform.position, enemyRoot.transform.rotation);

            // Apply knockback to all rigidbodies
            Rigidbody[] ragdollRigidbodies = brokenInstance.GetComponentsInChildren<Rigidbody>();
            Vector3 pushDirection = (enemyRoot.transform.position - weaponHitPoint.position).normalized;
            pushDirection.y = 0;

            foreach (Rigidbody rb in ragdollRigidbodies)
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.AddForce(pushDirection * knockbackForce, ForceMode.Impulse);
                }
            }

            Destroy(brokenInstance, enemyDestroyDelay); // Optional: clean up broken version after delay
        }

        // Instantly destroy the original enemy object
        Destroy(enemyRoot);
    }
}
