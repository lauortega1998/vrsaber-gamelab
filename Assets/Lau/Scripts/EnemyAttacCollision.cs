using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    public int damageAmount = 10;
    private PlayerHealth playerHealth;

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
                shield.TakeDamage(damageAmount);
                Debug.Log("Shield took damage! Player is safe.");
            }
            else
            {
                Debug.LogWarning("Hit a shield but no ShieldHealth component found!");
            }
            // No damage to player if shield detected
        }
        else if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player took damage directly!");
            }
        }
    }
}