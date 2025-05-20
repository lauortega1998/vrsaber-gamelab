using System;
using UnityEngine;
using System.Collections;

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
        yield return new WaitForSeconds(5f); //it should be 5f

        // Tutorial Level (1 min)
        Debug.Log("Tutorial started. Duration: 1 minute.");
        enemyspawnerLevelTutorial.SetActive(true);
        yield return new WaitForSeconds(60f); //it should be 60f 
        enemyspawnerLevelTutorial.SetActive(false);

        
        Debug.Log("Tutorial rage mode started (last 10 seconds).");
        yield return new WaitForSeconds(10f);   //it should be 10f  

        // Transition to Fire Level
        Debug.Log("Transition to Fire level...");
        yield return StartCoroutine(TransitionGroundColor(redColor));
        yield return new WaitForSeconds(10f); //it should be 10f 

        // Fire Level (2 min)
        Debug.Log("Fire level started. Duration: 2 minutes.");
        enemyspanwerLevelFire.SetActive(true);
        yield return new WaitForSeconds(120f); //it should be 120f
        enemyspanwerLevelFire.SetActive(false);

        // Transition to Ice Level
        Debug.Log("Transition to Ice level...");
        yield return StartCoroutine(TransitionGroundColor(blueColor));
        yield return new WaitForSeconds(10f); //it should be 10f

        // Ice Level (3 min)
        Debug.Log("Ice level started. Duration: 3 minutes.");
        enemyspanwerLevelIce.SetActive(true);
        yield return new WaitForSeconds(180f); //it should be 180f
        enemyspanwerLevelIce.SetActive(false);

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