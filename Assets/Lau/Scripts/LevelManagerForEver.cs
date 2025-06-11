using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndlessLevelManager : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyFactory;
    public TextMeshProUGUI countdownText;

    [Header("Post Processing")]
    public GameObject normalPostProcessing;
    public GameObject icePostProcessing;
    public GameObject firePostProcessing;

    [Header("Environmental Effects")]
    public GameObject rainEffect; // Fire = rain
    public GameObject snowEffect; // Ice = snow

    [Header("Timing")]
    public float initialDelay = 10f;
    public float cycleDuration = 90f;

    private bool isIce = true;
    public bool tutorial = false;

    void Start()
    {
        StartCoroutine(LevelLoop());
        tutorial = false;
    }

    private System.Collections.IEnumerator LevelLoop()
    {
        // INITIAL DELAY BEFORE START
        countdownText.gameObject.SetActive(true);
        yield return StartCoroutine(UpdateCountdown(initialDelay, ""));

        enemyFactory.SetActive(true);
        normalPostProcessing.SetActive(false);
        Debug.Log("Enemy factory activated. Starting environment cycle...");

        // START POST-PROCESSING CYCLE (ICE FIRST)
        while (true)
        {
            if (isIce)
            {
                ActivateIceEnvironment();
                yield return StartCoroutine(UpdateCountdown(cycleDuration, ""));
            }
            else
            {
                ActivateFireEnvironment();
                yield return StartCoroutine(UpdateCountdown(cycleDuration, ""));
            }

            isIce = !isIce;
        }
    }

    private void ActivateIceEnvironment()
    {
        Debug.Log("Activating Ice Environment");
        firePostProcessing.SetActive(false);
        icePostProcessing.SetActive(true);

        rainEffect.SetActive(false);
        snowEffect.SetActive(true);
    }

    private void ActivateFireEnvironment()
    {
        Debug.Log("Activating Fire Environment");
        icePostProcessing.SetActive(false);
        firePostProcessing.SetActive(true);

        snowEffect.SetActive(false);
        rainEffect.SetActive(true);
    }

    private System.Collections.IEnumerator UpdateCountdown(float duration, string prefix)
    {
        float timer = duration;
        while (timer > 0)
        {
            countdownText.text = prefix + Mathf.CeilToInt(timer).ToString();
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
        countdownText.text = "";
    }
}
