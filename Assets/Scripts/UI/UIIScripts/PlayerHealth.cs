using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // <- For reloading the scene
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public delegate void HealthChanged(int currentHealth, int maxHealth);
    public event HealthChanged OnHealthChanged;
    public TextMeshProUGUI healthText;

    private Color originalColor;
    private float originalFontSize;
    private Vector3 originalPosition;

    public Color damageColor = Color.red;
    public float damageFontSizeIncrease = 10f;
    public float flashDuration = 0.3f;

    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;

    public float vibrationIntensity = 5f; // How much the text vibrates
    public float vibrationDuration = 0.3f; // How long the vibration lasts

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        UpdateHealthUI();

        if (healthText != null)
        {
            originalColor = healthText.color;
            originalFontSize = healthText.fontSize;
            originalPosition = healthText.rectTransform.localPosition;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log($"Player took {damage} damage! Current health: {currentHealth}");

        UpdateHealthUI();

        if (healthText != null)
        {
            StartCoroutine(FlashHealthText());
            StartCoroutine(VibrateHealthText());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log($"Player healed {amount} health! Current health: {currentHealth}");
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + " / " + maxHealth;

            // Change color based on current health
            float healthPercent = (float)currentHealth / maxHealth;
            healthText.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
        }
    }

    private IEnumerator FlashHealthText()
    {
        // Make text bigger (no color change here)
        healthText.fontSize = originalFontSize + damageFontSizeIncrease;

        yield return new WaitForSeconds(flashDuration);

        // Restore original size
        healthText.fontSize = originalFontSize;
    }

    private IEnumerator VibrateHealthText()
    {
        float elapsed = 0f;

        while (elapsed < vibrationDuration)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-vibrationIntensity, vibrationIntensity),
                Random.Range(-vibrationIntensity, vibrationIntensity),
                0f
            );

            healthText.rectTransform.localPosition = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset to original position after vibration
        healthText.rectTransform.localPosition = originalPosition;
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}