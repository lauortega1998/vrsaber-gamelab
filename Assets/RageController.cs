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
    public GameObject fireHandUI;
    public GameObject iceHandEffectObject;
    public GameObject iceHandUI;

    [Header("Power Activation")]
    public InputPowers inputPowers;

    [Header("Post-Processing Volume")]
    public Volume postProcessingVolume;

    [Header("Post-Processing Settings (Rage Mode)")]
    public float rageVignetteIntensity = 0.85f;
    public float rageBloomIntensity = 15f;
    public float rageSaturationBoost = 40f;
    public Color rageColorFilter = new Color(1f, 0.3f, 0.3f);

    [Header("Post-Processing Settings (Normal Mode)")]
    public float normalVignetteIntensity = 0f;
    public float normalBloomIntensity = 0f;
    public float normalSaturation = 0f;
    public Color normalColorFilter = Color.white;

    private Vignette vignette;
    private Bloom bloom;
    private ColorAdjustments colorAdjustments;

    private bool isDepleting = false;

    private void Start()
    {
        if (postProcessingVolume != null)
        {
            postProcessingVolume.profile.TryGet(out vignette);
            postProcessingVolume.profile.TryGet(out bloom);
            postProcessingVolume.profile.TryGet(out colorAdjustments);

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

    private void StartDepletingRage()
    {
        isDepleting = true;
        ActivateRageEffects();
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
            colorAdjustments.saturation.value = rageSaturationBoost;
            colorAdjustments.colorFilter.value = rageColorFilter;
        }
    }

    private void DeactivateRageEffects()
    {
        if (fireHandEffectObject != null)
            fireHandEffectObject.SetActive(false);

        if (iceHandEffectObject != null)
            iceHandEffectObject.SetActive(false);

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
    }

    private void UpdateRageUI()
    {
        if (rageBar != null)
        {
            rageBar.value = currentRage / rageMax;
        }
    }
}

