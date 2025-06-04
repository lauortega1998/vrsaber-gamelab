using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class LevelManagerForEver : MonoBehaviour
{
    [Header("Ground Material")]
    public Renderer floorRenderer;
    public Color blackColor;
    public Color redColor;
    public Color blueColor;
    public float transitionDuration = 2f;

    public GameObject enemyFactory; // Only one enemy factory used throughout

    public TextMeshProUGUI countdownText;
    public GameObject iceLevelStarted;
    public GameObject fireLevelStarted;

    public GameObject normalPostProcessing;
    public GameObject icePostProcessing;
    public GameObject firePostProcessing;

    public GameObject rainEffect;
    public GameObject snowEffect;

    [Header("Scene Lighting")]
    public Transform bannerLight;
    public float newYRotation_Ice = 7f;
    public float newYRotation_Fire = 50f;

    void Awake()
    {
        StartCoroutine(LevelFlowLoop());
    }

    private IEnumerator LevelFlowLoop()
    {
        // Initial setup
        floorRenderer.material.color = blackColor;
        normalPostProcessing.SetActive(true);
        enemyFactory.SetActive(true);
        Debug.Log("Starting Endless Mode...");

        yield return new WaitForSeconds(10f);
        normalPostProcessing.SetActive(false);

        bool isIce = true;

        while (true)
        {
            if (isIce)
            {
                yield return StartCoroutine(SwitchToIceLevel());
            }
            else
            {
                yield return StartCoroutine(SwitchToFireLevel());
            }
            isIce = !isIce;
        }
    }

    private IEnumerator SwitchToIceLevel()
    {
        Debug.Log("Switching to Ice Level");

        // Transition
        yield return StartCoroutine(TransitionGroundColor(blueColor));

        // Setup Ice
        snowEffect.SetActive(true);
        rainEffect.SetActive(false);
        firePostProcessing.SetActive(false);
        icePostProcessing.SetActive(true);
        RotateBannerLightY(newYRotation_Ice);

        // UI and audio
        iceLevelStarted.SetActive(true);
        FindAnyObjectByType<AudioManager>()?.Play("Horn2");

        // Timer
        yield return StartCoroutine(ShowLevelTimer(10f));

        iceLevelStarted.SetActive(false);
    }

    private IEnumerator SwitchToFireLevel()
    {
        Debug.Log("Switching to Fire Level");

        // Transition
        yield return StartCoroutine(TransitionGroundColor(redColor));

        // Setup Fire
        snowEffect.SetActive(false);
        rainEffect.SetActive(true);
        icePostProcessing.SetActive(false);
        firePostProcessing.SetActive(true);
        RotateBannerLightY(newYRotation_Fire);

        // UI and audio
        fireLevelStarted.SetActive(true);
        FindAnyObjectByType<AudioManager>()?.Play("Rain");
        FindAnyObjectByType<AudioManager>()?.Play("Horn2");

        // Timer
        yield return StartCoroutine(ShowLevelTimer(10f));

        fireLevelStarted.SetActive(false);
    }

    private IEnumerator TransitionGroundColor(Color targetColor)
    {
        Material floorMat = floorRenderer.material;
        Color startColor = floorMat.color;
        Color black = blackColor;

        float halfDuration = transitionDuration / 2f;
        float time = 0f;

        // Step 1: Fade to black
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            floorMat.color = Color.Lerp(startColor, black, time / halfDuration);
            yield return null;
        }

        floorMat.color = black;
        yield return new WaitForSeconds(0.2f);

        // Step 2: Fade to target color
        time = 0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            floorMat.color = Color.Lerp(black, targetColor, time / halfDuration);
            yield return null;
        }

        floorMat.color = targetColor;
    }

    private IEnumerator ShowLevelTimer(float duration)
    {
        countdownText.gameObject.SetActive(true);
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            countdownText.text = Mathf.CeilToInt(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
    }

    private void RotateBannerLightY(float yRotation)
    {
        if (bannerLight == null) return;

        Vector3 currentRotation = bannerLight.eulerAngles;
        bannerLight.rotation = Quaternion.Euler(currentRotation.x, yRotation, currentRotation.z);
    }
}
