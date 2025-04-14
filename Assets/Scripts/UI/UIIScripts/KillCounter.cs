using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance;

    public int killCount = 0;
    public TextMeshProUGUI killCountText; // Assign a UI Text element here

    void Awake()
    {
        // Singleton pattern so other scripts can access this easily
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddKill()
    {
        killCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (killCountText != null)
            killCountText.text = "Score: " + killCount;
    }
}