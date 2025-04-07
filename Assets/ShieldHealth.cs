using UnityEngine;

public class ShieldHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"{gameObject.name} Shield Health: {currentHealth}/{maxHealth}");
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} Shield Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            DestroyShield();
        }
    }

    private void DestroyShield()
    {
        Debug.Log($"{gameObject.name} Shield has been destroyed!");
        Destroy(gameObject);
    }
}
