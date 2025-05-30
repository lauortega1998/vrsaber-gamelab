using System;
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
        yield return new WaitForSeconds(60f);
        enemyspawnerLevelTutorial.SetActive(false);

       

        // ICE LEVEL
        Debug.Log("Transition to Ice level...");
        yield return StartCoroutine(TransitionGroundColor(blueColor));

        Debug.Log("Countdown before Ice level...");
        yield return StartCoroutine(Countdown(5f)); // Countdown before Ice starts

        Debug.Log("Ice level started.");
        iceLevelStarted.SetActive(true);
        enemyspanwerLevelIce.SetActive(true);
        yield return new WaitForSeconds(120f); // 2 minutes
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
        yield return new WaitForSeconds(90f); // 1.5 minutes
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
    
}