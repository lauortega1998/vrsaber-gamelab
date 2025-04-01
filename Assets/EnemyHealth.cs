using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Hit Effect")]
    public GameObject hitEffectPrefab;

    [Header("Death Model Swap")]
    public string normalModelName = "NormalModel";
    public string destroyedModelName = "DestroyedModel";

    [Header("Death Effects")]
    public float knockbackForce = 5f;
    public float enemyDestroyDelay = 2f;

    public void Die(Transform weaponHitPoint)
    {
        GameObject enemyRoot = transform.root.gameObject;
        Debug.Log($"{enemyRoot.name} was killed!");

        // Spawn hit effect at closest point
        if (hitEffectPrefab != null && weaponHitPoint != null)
        {
            Vector3 hitPoint = GetComponent<Collider>().ClosestPoint(weaponHitPoint.position);
            GameObject effect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // Find enemy models
        Transform normalModel = enemyRoot.transform.Find(normalModelName);
        Transform destroyedModel = enemyRoot.transform.Find(destroyedModelName);

        if (normalModel != null && destroyedModel != null)
        {
            normalModel.gameObject.SetActive(false);
            destroyedModel.gameObject.SetActive(true);

            // Push ragdoll parts
            Rigidbody[] ragdollRigidbodies = destroyedModel.GetComponentsInChildren<Rigidbody>();
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

            // Destroy the enemy object after a delay
            Destroy(enemyRoot, enemyDestroyDelay);
        }
        else
        {
            Debug.LogWarning("NormalModel or DestroyedModel not found on enemy.");
            Destroy(enemyRoot); // Fallback: just destroy the enemy
        }
    }
}
