using UnityEngine;
using TMPro;

public class ScoreboardUI : MonoBehaviour
{
    public ScoreboardManager scoreboardManager;
    public TextMeshProUGUI text;

    private void Start()
    {
        ShowScores();
    }

    void ShowScores()
    {
        var scores = scoreboardManager.GetTopScores(10);
        text.text = "";

        foreach (var entry in scores)
        {
            text.text += $"{entry.playerName} - {entry.score} - {entry.date}\n";
        }
    }
}