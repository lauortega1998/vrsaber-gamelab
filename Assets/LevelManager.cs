using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;



public class LevelManager : MonoBehaviour
{


    [Header("Ground Material")]
    public Renderer floorRenderer;
    public Color blackColor;
    public Color redColor;
    public Color blueColor;
    public bool tutorial = false;
    
    public float transitionDuration = 2f;
    public GameObject enemyspawnerLevelTutorial;
    public GameObject enemyspanwerLevelFire;
    public GameObject enemyspanwerLevelIce;

    public TextMeshProUGUI countdownText;
    public GameObject gameCompleted;
    public GameObject iceLevelStarted;
    public GameObject fireLevelStarted;

    public GameObject normalPostProcessing;
    public GameObject icePostProcessing;
    public GameObject firePostProcessing;
    
    
    public GameObject rainEffect;
    public GameObject snowEffect;

    public GameObject winUI;              // Assign in inspector
    public TextMeshProUGUI finalScoreText; // Assign this in the Inspector
    public Image fadeImage;                 // Black UI Image for fade
    public float fadeDuration = 2f;
    public float delayBeforeLoad = 10f;

    [Header("Scene Lighting")]
    public Transform bannerLight;
    public float newYRotation_Tutorial = -42f;
    public float newYRotation_Ice = 7f;
    public float newYRotation_Fire = 50f;
    


    void Start()
    {
        // Initial ground color to black
        floorRenderer.material.color = blackColor;

        normalPostProcessing.SetActive(true);
        Debug.Log("Start: Black ground for 30 seconds. Waiting for Start Button.");
    }

    public void Awake()
    {
        OnStartButtonPressed();
    }



    public void OnStartButtonPressed() // CALL THIS FUNCTION IN THE START LOGIC.
    {
        Time.timeScale = 1f;
        StartCoroutine(LevelFlow());
        Debug.Log("game started");
    }

    private IEnumerator LevelFlow()
    {
        Debug.Log("Start button pressed. Starting in 5 seconds...");
        yield return new WaitForSeconds(5f);

        // Tutorial Level
        Debug.Log("Tutorial started. Duration: 1 minute.");
        RotateBannerLightY(newYRotation_Tutorial);

        enemyspawnerLevelTutorial.SetActive(true);
        tutorial = true;
        yield return StartCoroutine(ShowLevelTimer(45f)); // 45
        enemyspawnerLevelTutorial.SetActive(false);
        tutorial = false;

        yield return new WaitForSeconds(0f); // 60


        // ICE LEVEL
        Debug.Log("Transition to Ice level...");
        yield return StartCoroutine(TransitionGroundColor(blueColor));

        Debug.Log("Countdown before Ice level...");
        
        yield return StartCoroutine(Countdown(5f)); // Countdown before Ice starts
        normalPostProcessing.SetActive(false);
        RotateBannerLightY(newYRotation_Ice);

        snowEffect.SetActive(true);
        icePostProcessing.SetActive(true);
        Debug.Log("Ice level started.");

        iceLevelStarted.SetActive(true);
        FindAnyObjectByType<AudioManager>().Play("Horn2");
        enemyspanwerLevelIce.SetActive(true);
        yield return StartCoroutine(ShowLevelTimer(90f)); // 90
        enemyspanwerLevelIce.SetActive(false);
        KillAllEnemies();
        iceLevelStarted.SetActive(false);
        


        // FIRE LEVEL
        Debug.Log("Transition to Fire level...");
        yield return StartCoroutine(TransitionGroundColor(redColor));

        Debug.Log("Countdown before Fire level...");
        snowEffect.SetActive(false);
        yield return StartCoroutine(Countdown(5f)); // Countdown before Fire starts

        Debug.Log("Fire level started.");
        icePostProcessing.SetActive(false);
        RotateBannerLightY(newYRotation_Fire);

        fireLevelStarted.SetActive(true);
        firePostProcessing.SetActive(true);
        rainEffect.SetActive(true);
        FindAnyObjectByType<AudioManager>().Play("Rain");
        enemyspanwerLevelFire.SetActive(true);
        FindAnyObjectByType<AudioManager>().Play("Horn2");
        yield return StartCoroutine(ShowLevelTimer(120f)); // 120
        enemyspanwerLevelFire.SetActive(false);
        fireLevelStarted.SetActive(false);

        // Game Completed
        Debug.Log("Game completed!");
        KillAllEnemies();

        firePostProcessing.SetActive(false);
        fireLevelStarted.SetActive(false);
        normalPostProcessing.SetActive(true);
        gameCompleted.SetActive(true);
        StartCoroutine(HandleWinSequence());

    }
    private IEnumerator HandleWinSequence()
    {
        // Update final score
        if (finalScoreText != null)
        {
            finalScoreText.text = KillCounter.Instance.killCount.ToString();
        }

        // Show death UI
        if (winUI != null)
            winUI.SetActive(true);

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

        // Optional: hold on black briefly
        yield return new WaitForSeconds(0.2f);

        // Step 2: Fade from black to target
        time = 0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            floorMat.color = Color.Lerp(black, targetColor, time / halfDuration);
            yield return null;
        }

        floorMat.color = targetColor;
    }

    private IEnumerator Countdown(float seconds)
    {
        countdownText.gameObject.SetActive(true);
        while (seconds > 0)
        {
            countdownText.text = Mathf.Ceil(seconds).ToString();
            yield return new WaitForSeconds(1f);
            seconds--;
        }

        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
    }
    private IEnumerator ShowLevelTimer(float duration)
    {
        countdownText.gameObject.SetActive(true);

        float timeLeft = duration;
        while (timeLeft > 0)
        {
            TimeSpan t = TimeSpan.FromSeconds(timeLeft);
            countdownText.text = Mathf.CeilToInt(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
    }

    private void KillAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        Debug.Log($"Force-destroyed {enemies.Length} enemies.");
    }
    private void RotateBannerLightY(float yRotation)
    {
        if (bannerLight == null) return;

        Vector3 currentRotation = bannerLight.eulerAngles;
        bannerLight.rotation = Quaternion.Euler(currentRotation.x, yRotation, currentRotation.z);
    }

}

/*using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class LevelManager : MonoBehaviour
{
   

    [Header("Ground Material")]
    public Renderer floorRenderer;
    public Color blackColor;
    public Color redColor;
    public Color blueColor;
    public float transitionDuration = 2f;
    public GameObject enemyspawnerLevelTutorial;
    public GameObject enemyspanwerLevelFire;
    public GameObject enemyspanwerLevelIce;

    public TextMeshProUGUI countdownText;
    public GameObject gameCompleted;
    public GameObject iceLevelStarted;
    public GameObject fireLevelStarted;
    
    
    void Start()
    {
        // Initial ground color to black
        floorRenderer.material.color = blackColor;

        
        Debug.Log("Start: Black ground for 30 seconds. Waiting for Start Button.");
    }

    public void Awake()
    {
        OnStartButtonPressed();
    }



    public void OnStartButtonPressed() // CALL THIS FUNCTION IN THE START LOGIC.
    {
        Time.timeScale = 1f;
        StartCoroutine(LevelFlow());
        Debug.Log("game started");
    }

    private IEnumerator LevelFlow()
    {
        Debug.Log("Start button pressed. Starting in 5 seconds...");
        yield return new WaitForSeconds(5f);

        // Tutorial Level
        Debug.Log("Tutorial started. Duration: 1 minute.");
        enemyspawnerLevelTutorial.SetActive(true);
        yield return new WaitForSeconds(5); //60
        enemyspawnerLevelTutorial.SetActive(false);

        yield return new WaitForSeconds(5f); // 60


        // ICE LEVEL
        Debug.Log("Transition to Ice level...");
        yield return StartCoroutine(TransitionGroundColor(blueColor));

        Debug.Log("Countdown before Ice level...");
        yield return StartCoroutine(Countdown(5f)); // Countdown before Ice starts

        Debug.Log("Ice level started.");
        iceLevelStarted.SetActive(true);
        enemyspanwerLevelIce.SetActive(true);
        yield return new WaitForSeconds(5f); // 120 
        enemyspanwerLevelIce.SetActive(false);
        iceLevelStarted.SetActive(false);

        // FIRE LEVEL
        Debug.Log("Transition to Fire level...");
        yield return StartCoroutine(TransitionGroundColor(redColor));

        Debug.Log("Countdown before Fire level...");
        yield return StartCoroutine(Countdown(5f)); // Countdown before Fire starts

        Debug.Log("Fire level started.");
        fireLevelStarted.SetActive(true);
        enemyspanwerLevelFire.SetActive(true);
        yield return new WaitForSeconds(5f); // 180
        enemyspanwerLevelFire.SetActive(false);
        fireLevelStarted.SetActive(false);

        // Game Completed
        Debug.Log("Game completed!");
        gameCompleted.SetActive(true);
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

        // Optional: hold on black briefly
        yield return new WaitForSeconds(0.2f);

        // Step 2: Fade from black to target
        time = 0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            floorMat.color = Color.Lerp(black, targetColor, time / halfDuration);
            yield return null;
        }

        floorMat.color = targetColor;
    }
    
    private IEnumerator Countdown(float seconds)
    {
        countdownText.gameObject.SetActive(true);
        while (seconds > 0)
        {
            countdownText.text = Mathf.Ceil(seconds).ToString();
            yield return new WaitForSeconds(1f);
            seconds--;
        }

        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
    }
    
}*/