using UnityEngine;

public class SpectreProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spectre projectile hit the player!");
            Destroy(gameObject); // Remove projectile on impact
        }
    }
}

