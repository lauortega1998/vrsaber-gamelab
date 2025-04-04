using System;
using UnityEngine;

public class SpectreProjectile : MonoBehaviour
{
    public int damageAmount = 10; // How much damage the enemy deals
    private PlayerHealth playerHealth;


    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log("Spectre projectile hit the player!");
            Destroy(gameObject); // Remove projectile on impact
        }
    }
}

