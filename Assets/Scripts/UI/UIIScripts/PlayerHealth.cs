using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public delegate void HealthChanged(int currentHealth, int maxHealth);
    public event HealthChanged OnHealthChanged;

    [Header("UI References")]
    public Slider healthSlider; // Reference to the Slider component
    public TextMeshProUGUI healthText; // Optional text display

    [Header("Text Flash Settings")]
    public Color damageColor = Color.red;
    public float damageFontSizeIncrease = 10f;
    public float flashDuration = 0.3f;

    [Header("Health Bar Color Gradient")]
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;

    [Header("Vibration Settings")]
    public float vibrationIntensity = 5f;
    public float vibrationDuration = 0.3f;

    private Color originalColor;
    private float originalFontSize;
    private Vector3 originalPosition;
    private int damageSoundIndex = 0;


    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            originalColor = healthText.color;
            originalFontSize = healthText.fontSize;
            originalPosition = healthText.rectTransform.localPosition;
        }

        UpdateHealthUI();
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
            PlayNextDamageSound();

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
        float healthPercent = (float)currentHealth / maxHealth;

        // Update the slider value
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;

            // If the slider has a Fill image, change its color
            Image fillImage = healthSlider.fillRect?.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
            }
        }

        // Optional: keep or remove text
        if (healthText != null)
        {
            healthText.text = $"Health: ";
            healthText.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
        }
    }

    private IEnumerator FlashHealthText()
    {
        healthText.fontSize = originalFontSize + damageFontSizeIncrease;
        yield return new WaitForSeconds(flashDuration);
        healthText.fontSize = originalFontSize;
    }

    private IEnumerator VibrateHealthText()
    {
        float elapsed = 0f;

        while (elapsed < vibrationDuration)
        {
            Vector3 offset = new Vector3(
                UnityEngine.Random.Range(-vibrationIntensity, vibrationIntensity),
                UnityEngine.Random.Range(-vibrationIntensity, vibrationIntensity),
                0f
            );

            healthText.rectTransform.localPosition = originalPosition + offset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        healthText.rectTransform.localPosition = originalPosition;
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void PlayNextDamageSound()
    {
        string soundName = $"Damage{damageSoundIndex + 1}";
        FindAnyObjectByType<AudioManager>()?.Play(soundName);
        damageSoundIndex = (damageSoundIndex + 1) % 3; // Loops 0 → 1 → 2 → 0...
    }
}
