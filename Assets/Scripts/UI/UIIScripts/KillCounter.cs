using TMPro;
using UnityEngine;
using System.Collections;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance;

    public int killCount = 0;
    public TextMeshProUGUI killCountText;

    private Color originalColor;
    public Color increaseColor = Color.green;
    public Color decreaseColor = Color.red;
    public float flashDuration = 0.5f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (killCountText != null)
            originalColor = killCountText.color;
    }

    public void AddKill(int points)
    {
        killCount += points;
        killCount = Mathf.Max(killCount, 0);
        UpdateUI();

        if (points > 0)
            StartCoroutine(FlashTextColor(increaseColor));
        else if (points < 0)
            StartCoroutine(FlashTextColor(decreaseColor));
    }

    void UpdateUI()
    {
        if (killCountText != null)
            killCountText.text = killCount.ToString();
    }

    IEnumerator FlashTextColor(Color flashColor)
    {
        killCountText.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        killCountText.color = originalColor;
    }
}
