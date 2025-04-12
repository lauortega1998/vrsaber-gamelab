using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Delay Settings")]
    [SerializeField] private float actionDelay = 2.0f; // Delay after rune smash before action happens

    [Header("Play Settings")]
    [SerializeField] private GameObject menuScreen; // The current menu UI
    [SerializeField] private GameObject countdownUI; // The main game UI
    [SerializeField] private GameObject enemyFactory; // The main game UI
    [SerializeField] private GameObject torches; // The main game UI



    [Header("Scoreboard Settings")]
    [SerializeField] private GameObject menuScreenForScoreboard; // The current menu UI
    [SerializeField] private GameObject scoreboardScreen; // The scoreboard UI

    [Header("Go Back Settings")]
    [SerializeField] private GameObject[] screensToCloseOnBack;
    [SerializeField] private GameObject mainMenuScreen;

    [Header("Rune References")]
    [SerializeField] private GameObject[] allRunes;

    // Called externally when runes are smashed
    public void OnPlayRuneSmashed()
    {
        StartCoroutine(DelayedPlayAction());
    }

    public void OnExitRuneSmashed()
    {
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
                rune.SetActive(true);
        }
    }

    private IEnumerator DelayedPlayAction()
    {
        yield return new WaitForSeconds(actionDelay);
        if (menuScreen != null) menuScreen.SetActive(false);
        if (countdownUI != null) countdownUI.SetActive(true);
        if (enemyFactory != null) enemyFactory.SetActive(true);
        if (torches != null) torches.SetActive(true);
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
    }
}
