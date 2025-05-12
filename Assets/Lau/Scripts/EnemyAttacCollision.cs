using UnityEngine;

public class EnemyAttacCollision : MonoBehaviour
{
    public int normalEnemyDamage = 10;
    public int heavyEnemyDamage = 30;

    private PlayerHealth playerHealth;
    private bool shieldInside = false;
    private bool wallInside = false;

    private EnemyType currentEnemy;

    public GameObject shieldBlockEffect;
/*
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {



        if (other.CompareTag("Shield"))
        {
            ShieldHealth shield = other.GetComponent<ShieldHealth>();

            if (shield != null)
            {
                    shield.TakeDamage(10);

                    Debug.Log("Shield took damage!");

                    if (shieldBlockEffect != null)
                    {
                        Instantiate(shieldBlockEffect, transform.position, Quaternion.identity);
                    }

            }
            else
            {
                Debug.LogWarning("Hit a shield but no ShieldHealth component found!");
            }

            shieldInside = true;
        }
        else if (other.CompareTag("MovementStopper"))
        {
            wallInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            shieldInside = false;
        }
        else if (other.CompareTag("MovementStopper"))
        {
            wallInside = false;
        }
    }

    private void Update()
    {
        // Check once per frame if the attack is happening
        if (wallInside)
        {
            if (!shieldInside)
            {

                // Wall is inside but no shield to protect -> player takes damage

                if (playerHealth != null)
                {

                    playerHealth.TakeDamage(10);
                    Debug.Log("Player took damage because no shield was protecting!");
                }

                // After dealing damage once, reset
                wallInside = false;
            }
            else
            {
                Debug.Log("Shield blocked the player. No damage taken.");
                wallInside = false; // Reset wall so it doesn't keep checking
            }
        }
    }
    public void ResetProtectionStatus()
    {
        shieldInside = false;
        wallInside = false;
    }
    */





    
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is an enemy
        if (other.CompareTag("Enemy"))
        {
            // Check for EnemyDamage component
            EnemyDamage enemyDamage = other.GetComponent<EnemyDamage>();
            if (enemyDamage != null)
            {
                HandleEnemyDamage(enemyDamage.damageAmount, other);
                Debug.Log($"Enemy hit! Damage dealt: {enemyDamage.damageAmount}");
            }
            else
            {
                Debug.LogWarning("EnemyDamage component missing on enemy! Using fallback value.");
                HandleEnemyDamage(normalEnemyDamage, other);
            }
        }

        // Handle Shield interaction
        if (other.CompareTag("Shield"))
        {
            ShieldHealth shield = other.GetComponent<ShieldHealth>();
            if (shield != null)
            {
                shield.TakeDamage(10); // Fixed shield damage
                Debug.Log("Shield took damage!");

                if (shieldBlockEffect != null)
                {
                    Instantiate(shieldBlockEffect, transform.position, Quaternion.identity);
                }
            }
            else
            {
                Debug.LogWarning("Hit a shield but no ShieldHealth component found!");
            }

            shieldInside = true;
        }
        else if (other.CompareTag("MovementStopper"))
        {
            wallInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            shieldInside = false;
        }
        else if (other.CompareTag("MovementStopper"))
        {
            wallInside = false;
        }
    }

    private void Update()
    {
        if (wallInside)
        {
            if (!shieldInside)
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(normalEnemyDamage);
                    Debug.Log("Player took damage because no shield was protecting!");
                }

                wallInside = false;
            }
            else
            {
                Debug.Log("Shield blocked the player. No damage taken.");
                wallInside = false;
            }
        }
    }

    private void HandleEnemyDamage(int damageAmount, Collider enemyCollider)
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log($"Player took {damageAmount} damage from {enemyCollider.gameObject.name}");
        }
    }

    public void ResetProtectionStatus()
    {
        shieldInside = false;
        wallInside = false;
    }
}