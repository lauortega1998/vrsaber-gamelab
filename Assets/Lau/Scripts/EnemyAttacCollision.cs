using UnityEngine;

public class EnemyAttacCollision : MonoBehaviour
{
   

    private PlayerHealth playerHealth;
    private bool shieldInside = false;
    private bool wallInside = false;
    private EnemyType currentEnemy;

    public GameObject shieldBlockEffect;
    public LevelManager levelManager;
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        levelManager = FindObjectOfType<LevelManager>();

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
        if (wallInside && levelManager != null && !levelManager.tutorial)
        {
            if (!shieldInside)
            {

                // Wall is inside but no shield to protect -> player takes damage

                if (playerHealth != null )
                {

                    playerHealth.TakeDamage(10);
                    Debug.Log("Player took damage because no shield was protecting!");
                }

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
    





    
    
}