using UnityEngine;
using TMPro; // TextMeshPro

public class ScoreSubmitter : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public ScoreboardManager scoreboardManager;
    public int score; // Your actual game score

    public void SubmitScore()
    {
        string playerName = playerNameInput.text;
        scoreboardManager.AddScore(playerName, score);
    }
}