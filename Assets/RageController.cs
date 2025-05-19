using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RageSystem : MonoBehaviour
{
    [Header("Rage Settings")]
    public float currentRage = 0f;
    public float rageIncreasePerKill = 20f;
    public float rageMax = 100f;
    public float rageDepletionRate = 10f;

    [Header("UI Settings")]
    public Slider rageBar;

    [Header("Rage Effects")]
    public GameObject fireHandEffectObject;
    public GameObject fireHandEffectPower;

    public GameObject fireHandUI;
    public GameObject iceHandEffectObject;
    public GameObject iceHandEffectPower;

    public GameObject iceHandUI;

    [Header("Power Activation")]
    public InputPowers inputPowers;

    [Header("Post-Processing Volume")]
    public Volume postProcessingVolume;

    [Header("Post-Processing Settings (Rage Mode)")]
    public float rageVignetteIntensity = 0.85f;
    public float rageBloomIntensity = 15f;
    public float rageSaturationBoost = 0f; // Removed saturation boost
    public Color rageColorFilter = new Color(1f, 0.3f, 0.3f);
    public float rageFilmGrainIntensity = 0.5f; // Added film grain intensity

    [Header("Post-Processing Settings (Normal Mode)")]
    public float normalVignetteIntensity = 0f;
    public float normalBloomIntensity = 0f;
    public float normalSaturation = 0f;
    public Color normalColorFilter = Color.white;
    public float normalFilmGrainIntensity = 0f; // Normal film grain intensity

    [Header("Rage Haptics")]
    [Tooltip("Strength of each pulse when Rage is triggered (0–1)")]
    public float rageHapticAmplitude = 0.7f;
    [Tooltip("Duration of each pulse in seconds")]
    public float rageHapticDuration = 0.1f;
    [Tooltip("Gap between the two pulses (in seconds, must be <1)")]
    public float rageHapticInterval = 0.5f;

    private Vignette vignette;
    private Bloom bloom;
    private ColorAdjustments colorAdjustments;
    private FilmGrain filmGrain;

    private bool isDepleting = false;

    private void Start()
    {
        if (postProcessingVolume != null)
        {
            postProcessingVolume.profile.TryGet(out vignette);
            postProcessingVolume.profile.TryGet(out bloom);
            postProcessingVolume.profile.TryGet(out colorAdjustments);
            postProcessingVolume.profile.TryGet(out filmGrain);

            ResetPostProcessing(); // Start with normal settings
        }

        if (fireHandEffectObject != null)
            fireHandEffectObject.SetActive(false);

        if (iceHandEffectObject != null)
            iceHandEffectObject.SetActive(false);
    }

    private void Update()
    {
        if (isDepleting)
        {
            DepleteRage();
        }

        UpdateRageUI();
    }

    public void OnEnemyKilled()
    {
        if (!isDepleting)
        {
            currentRage += rageIncreasePerKill;
            currentRage = Mathf.Clamp(currentRage, 0, rageMax);

            if (currentRage >= rageMax)
            {
                StartDepletingRage();
            }
        }
    }

    public void StartDepletingRage()
    {
        isDepleting = true;
        ActivateRageEffects();

        // Haptics implementation to let the players know that the rage-meter is full and ready to use 
        HapticsManager.Instance.TriggerHaptics(rageHapticAmplitude, rageHapticDuration); // first pulse in the controllers

        Invoke(nameof(SecondRagePulse), rageHapticInterval); // Unity waits rageHapticInterval seconds and the second pulse in the controllers
    }

    private void SecondRagePulse()
    {
        HapticsManager.Instance.TriggerHaptics(rageHapticAmplitude, rageHapticDuration);
    }

    private void DepleteRage()
    {
        currentRage -= rageDepletionRate * Time.deltaTime;
        currentRage = Mathf.Clamp(currentRage, 0, rageMax);

        if (currentRage <= 0)
        {
            StopDepletingRage();
        }
    }

    private void StopDepletingRage()
    {
        isDepleting = false;
        DeactivateRageEffects();
    }

    private void ActivateRageEffects()
    {
        if (fireHandEffectObject != null)
            fireHandEffectObject.SetActive(true);

        if (iceHandEffectObject != null)
            iceHandEffectObject.SetActive(true);

        if (fireHandUI != null)
            fireHandUI.SetActive(true);

        if (iceHandUI != null)
            iceHandUI.SetActive(true);

        if (inputPowers != null)
            inputPowers.enabled = true;

        // Set Rage Post-processing Settings
        if (vignette != null)
            vignette.intensity.value = rageVignetteIntensity;

        if (bloom != null)
            bloom.intensity.value = rageBloomIntensity;

        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = rageSaturationBoost; // Removed saturation boost
            colorAdjustments.colorFilter.value = rageColorFilter;
        }

        if (filmGrain != null)
            filmGrain.intensity.value = rageFilmGrainIntensity; // Added film grain intensity
    }

    private void DeactivateRageEffects()
    {
        if (fireHandEffectObject != null)
            fireHandEffectObject.SetActive(false);

        if (fireHandEffectPower != null)
            fireHandEffectPower.SetActive(false);

        if (iceHandEffectObject != null)
            iceHandEffectObject.SetActive(false);

        if (iceHandEffectPower != null)
            iceHandEffectPower.SetActive(false);

        if (fireHandUI != null)
            fireHandUI.SetActive(false);

        if (iceHandUI != null)
            iceHandUI.SetActive(false);

        if (inputPowers != null)
            inputPowers.enabled = false;

        ResetPostProcessing();
    }

    private void ResetPostProcessing()
    {
        if (vignette != null)
            vignette.intensity.value = normalVignetteIntensity;

        if (bloom != null)
            bloom.intensity.value = normalBloomIntensity;

        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = normalSaturation;
            colorAdjustments.colorFilter.value = normalColorFilter;
        }

        if (filmGrain != null)
            filmGrain.intensity.value = normalFilmGrainIntensity; // Normal film grain intensity
    }

    private void UpdateRageUI()
    {
        if (rageBar != null)
        {
            rageBar.value = currentRage / rageMax;
        }
    }
}

