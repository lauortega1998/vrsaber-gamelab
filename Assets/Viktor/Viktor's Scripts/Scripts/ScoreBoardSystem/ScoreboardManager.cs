using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreboardManager : MonoBehaviour
{
    private string savePath;
    public ScoreboardData scoreboardData = new ScoreboardData();

    private void Awake()
    {
        // Where the score file will be saved
        savePath = Path.Combine(Application.persistentDataPath, "scoreboard.json");
        LoadScoreboard();
    }

    public void AddScore(string playerName, int score)
    {
        ScoreEntry newScore = new ScoreEntry
        {
            playerName = playerName,
            score = score,
            date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        scoreboardData.entries.Add(newScore);

        // Sort scores from highest to lowest
        scoreboardData.entries.Sort((a, b) => b.score.CompareTo(a.score));

        SaveScoreboard();
    }

    private void SaveScoreboard()
    {
        string json = JsonUtility.ToJson(scoreboardData, true);
        File.WriteAllText(savePath, json);
    }

    private void LoadScoreboard()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            scoreboardData = JsonUtility.FromJson<ScoreboardData>(json);
        }
    }

    public List<ScoreEntry> GetTopScores(int count)
    {
        return scoreboardData.entries.GetRange(0, Mathf.Min(count, scoreboardData.entries.Count));
    }
}