using System;
using UnityEngine;

public class FlyingEnemyHealth : MonoBehaviour
{
    [Header("Hit Effect")]
    public GameObject hitEffectPrefab;

   

    [Header("Death Effects")]
    public float knockbackForce = 5f;
    public float enemyDestroyDelay = 2f;

    public event Action OnDeath;

    public void FylingEnemyDie(Transform weaponHitPoint)
    {
        //KillCounter.Instance.AddKill();
        OnDeath?.Invoke();
        Destroy(gameObject);


        // Spawn hit effect at closest point
        if (hitEffectPrefab != null && weaponHitPoint != null)
        {
            Vector3 hitPoint = GetComponent<Collider>().ClosestPoint(weaponHitPoint.position);
            GameObject effect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // Find enemy models

    



        else
        {

            Debug.LogWarning("NormalModel or DestroyedModel not found on enemy.");
        }
    }
}
