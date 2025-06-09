using System;
using UnityEngine;

public class SpectreProjectile : MonoBehaviour
{
    public int damageAmount = 10;
    private PlayerHealth playerHealth;
    public GameObject impactEffectPrefab;
    public GameObject brokenProjectilePrefab;
    public LevelManager levelManager;
    public EndlessLevelManager endlessLevelManager;
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        Destroy(gameObject, 3.5f);
        levelManager = FindObjectOfType<LevelManager>();
        endlessLevelManager = FindObjectOfType<EndlessLevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ProjectileDamage"))
        {
            if (levelManager != null && !levelManager.tutorial )
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
                
            }
            if (endlessLevelManager != null && !endlessLevelManager.tutorial )
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
                
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("ParryArea") || other.CompareTag("Shield"))

        {
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            }

            if (brokenProjectilePrefab != null)
            {
                GameObject brokenInstance = Instantiate(brokenProjectilePrefab, transform.position, transform.rotation);

                Rigidbody[] rigidbodies = brokenInstance.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rigidbodies)
                {
                    if (rb != null)
                    {
                        rb.AddExplosionForce(300f, transform.position, 2f);
                    }
                }

                Destroy(brokenInstance, 1.5f);
            }

            Debug.Log("Spectre projectile hit a weapon!");
            Destroy(gameObject);
        }
    }
}

