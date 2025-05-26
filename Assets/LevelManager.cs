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

    public GameObject levelStartUIIce;
    public GameObject levelEndUIIce;
    public GameObject countdownIce;

    public GameObject levelStartUIFire;
    public GameObject gameEndingUI;
    public GameObject countdownTextFire;

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
        // Wait 5 seconds after pressing Start
        Debug.Log("Start button pressed. Starting in 5 seconds...");
        yield return new WaitForSeconds(5f); // This will wait for 5 seconds before starting thetutorial

        // Tutorial Level (1 min)
        Debug.Log("Tutorial started. Duration: 1 minute.");
        enemyspawnerLevelTutorial.SetActive(true);
        yield return new WaitForSeconds(10f); // Wait for 50 seconds in the tutorial level
        enemyspawnerLevelTutorial.SetActive(false);
        levelStartUIIce.SetActive(true); // Show the UI for Ice level after the tutorial
        countdownIce.SetActive(true); // Activate countdown UI


        // Transition to Ice Level
        Debug.Log("Transition to Ice level...");
        yield return StartCoroutine(TransitionGroundColor(redColor)); // Transition ground color to red
        yield return new WaitForSeconds(10f); // Wait for 10 seconds for the countdown to appear
        levelStartUIIce.SetActive(false); // Deactivate the Ice Start UI
        countdownIce.SetActive(false); // Deactivate the countdown UI

        // Ice Level (1.5 min)
        Debug.Log("Ice level started. Duration: 90 seconds.");
        enemyspanwerLevelIce.SetActive(true); // Start spawning enemies for Ice level
        yield return new WaitForSeconds(10f); // Wait for 3 minutes for the Ice level
        enemyspanwerLevelIce.SetActive(false); // Stop spawning enemies for Ice level
        levelStartUIFire.SetActive(true); // Show UI for Fire level after Ice level ends

        // Transition to Fire Level
        Debug.Log("Transition to Fire level...");
        yield return StartCoroutine(TransitionGroundColor(blueColor)); // Transition ground color to blue
        levelEndUIIce.SetActive(true); // Show the end UI for Ice level
        countdownTextFire.SetActive(true); // Show countdown UI for Fire level
        yield return new WaitForSeconds(10f); // Wait for 10 seconds before starting the Fire level UI
        levelStartUIFire.SetActive(false); // Hide the Fire Start UI
        countdownTextFire.SetActive(false); // Hide the Fire countdown UI
        levelEndUIIce.SetActive(false); // Hide the Ice End UI

        // Fire Level (2 min)
        Debug.Log("Fire level started. Duration: 2 minutes.");
        enemyspanwerLevelFire.SetActive(true); // Start spawning enemies for Fire level
        yield return new WaitForSeconds(10f); // Wait for 2 minutes for the Fire level
        enemyspanwerLevelFire.SetActive(false); // Stop spawning enemies for Fire level
        gameEndingUI.SetActive(true); // Show the game ending UI

        Debug.Log("Game completed!");
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
}
