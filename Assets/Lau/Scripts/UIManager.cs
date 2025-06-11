using UnityEngine;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Delay Settings")]
    [SerializeField] private float actionDelay = 2.0f; // Delay after rune smash before action happens
    [SerializeField] private float gameCountDown = 5.0f; // Delay after rune smash before action happens


    [Header("Play Settings")]
    [SerializeField] private GameObject menuScreen; // The current menu UI
    [SerializeField] private GameObject levelName; // The current menu UI
    [SerializeField] private TMP_Text countdownUI; // The main game UI
    [SerializeField] private GameObject enemyFactory; // The main game UI
    [SerializeField] private GameObject enemyTutorial; // The main game UI

    [SerializeField] private GameObject torches; // The main game UI
    [SerializeField] private GameObject allMenuRunes; // The current menu UI


    public SmashableUI smashableUI;
    public LevelManager levelManager;
    
    [Header("Scoreboard Settings")]
    [SerializeField] private GameObject menuScreenForScoreboard; // The current menu UI
    [SerializeField] private GameObject scoreboardScreen; // The scoreboard UI
    [SerializeField] private GameObject goBackButton; // The scoreboard UI


    [Header("Go Back Settings")]
    [SerializeField] private GameObject[] screensToCloseOnBack;
    [SerializeField] private GameObject mainMenuScreen;

    [Header("Rune References")]
    [SerializeField] private GameObject[] allRunes;

    // Called externally when runes are smashed
    public void OnPlayRuneSmashed()
    {

        StartCoroutine(DelayedGameStart());
    }

    public void OnExitRuneSmashed()
    {
        StartCoroutine(DelayedPlayAction());
        StartCoroutine(DelayedExitAction());
    }

    public void OnScoreboardRuneSmashed()
    {
        StartCoroutine(DelayedScoreboardAction());
    }
    public void OnGoBackRuneSmashed()
    {
        foreach (GameObject screen in screensToCloseOnBack)
        {
            if (screen != null)
                screen.SetActive(false);
        }

        if (mainMenuScreen != null)
            mainMenuScreen.SetActive(true);

        // Reactivate all runes
        foreach (GameObject rune in allRunes)
        {
            if (rune != null)
            {
                rune.SetActive(true);

                SmashableUI smashable = rune.GetComponent<SmashableUI>();
                if (smashable != null)
                {
                    smashable.ResetRune();
                }
            }
        }

    }

    private IEnumerator DelayedPlayAction()
    {
        yield return new WaitForSeconds(actionDelay);
        if (menuScreen != null) menuScreen.SetActive(false);
        //if (countdownUI != null) countdownUI.SetActive(true);
        if (torches != null) torches.SetActive(true);
        Destroy(allMenuRunes);

    }

    private IEnumerator DelayedGameStart()
    {
        float countdown = gameCountDown;

        if (countdownUI != null)
            levelManager.OnStartButtonPressed();

        levelName.gameObject.SetActive(true);
            countdownUI.gameObject.SetActive(true); // Make sure the text is visible

        while (countdown > 0)
        {
            if (countdownUI != null)
                countdownUI.text = Mathf.CeilToInt(countdown).ToString(); // Show the countdown rounded up

            yield return null; // Wait for the next frame
            countdown -= Time.deltaTime; // Subtract the time passed since last frame
        }

        if (countdownUI != null)
        {
            countdownUI.text = "Go!"; // Optionally show "Go!" when countdown finishes
            yield return new WaitForSeconds(1f); // Wait a moment before hiding
            countdownUI.gameObject.SetActive(false);
        }

        if (countdownUI != null)
            levelName.gameObject.SetActive(false);
        countdownUI.gameObject.SetActive(false);

        if (enemyFactory != null)
            enemyFactory.SetActive(true);
        Destroy(allMenuRunes);

    }

    private IEnumerator DelayedExitAction()
    {
        yield return new WaitForSeconds(actionDelay);
        Debug.Log("Exiting the game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For editor testing
#endif
    }

    private IEnumerator DelayedScoreboardAction()
    {
        yield return new WaitForSeconds(actionDelay);
        if (menuScreenForScoreboard != null) menuScreenForScoreboard.SetActive(false);
        if (scoreboardScreen != null) scoreboardScreen.SetActive(true);
        if (scoreboardScreen != null) goBackButton.SetActive(true);

    }


}
