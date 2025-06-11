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
    public GameObject uiToDeactivate1;
    public GameObject uiToDeactivate2;
    public GameObject uiToDeactivate3;



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
    private bool lowHealthWarningPlayed = false;

    public GameObject deathUI;              // Assign in inspector
    public TextMeshProUGUI finalScoreText; // Assign this in the Inspector
    public Image fadeImage;                 // Black UI Image for fade
    public float fadeDuration = 2f;
    public float delayBeforeLoad = 2f;

    private bool hasDied = false;


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

        // Subtract 2 points from score
        KillCounter.Instance?.AddKill(-2);

        UpdateHealthUI();

        if (healthText != null)
        {
            StartCoroutine(FlashHealthText());
            StartCoroutine(VibrateHealthText());
            PlayNextDamageSound();
        }

        float healthPercent = (float)currentHealth / maxHealth;
        if (healthPercent <= 0.3f && !lowHealthWarningPlayed)
        {
            FindAnyObjectByType<AudioManager>()?.Play("low hp");
            lowHealthWarningPlayed = true;
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
    private IEnumerator HandleDeathSequence()
    {
        // Update final score
        if (finalScoreText != null)
        {
            finalScoreText.text = KillCounter.Instance.killCount.ToString();
        }

        // Show death UI
        if (deathUI != null)
            deathUI.SetActive(true);

        // Start fade
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
            {
                float normalizedTime = t / fadeDuration;
                color.a = Mathf.Lerp(0f, 1f, normalizedTime);
                fadeImage.color = color;
                yield return null;
            }

            // Ensure it's fully opaque
            color.a = 1f;
            fadeImage.color = color;
        }

        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene("MountainMenu"); // Replace with actual scene name or index
    }
    public void Die()
    {
        if (hasDied) return; // Prevent multiple triggers
        hasDied = true;

        Debug.Log("Player Died!");
        StartCoroutine(HandleDeathSequence());
        uiToDeactivate1.SetActive(false);
        uiToDeactivate2.SetActive(false);
        uiToDeactivate3.SetActive(false);

    }
    private void PlayNextDamageSound()
    {
        string soundName = $"damage{damageSoundIndex + 1}";
        FindAnyObjectByType<AudioManager>()?.Play(soundName);
        damageSoundIndex = (damageSoundIndex + 1) % 3; // Loops 0 → 1 → 2 → 0...
    }
}
