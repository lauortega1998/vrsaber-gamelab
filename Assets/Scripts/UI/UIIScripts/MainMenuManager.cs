using UnityEngine;
using System.Collections;
using TMPro;


public class ActivateObject : MonoBehaviour
{
    public GameObject enemyFactory;
    public GameObject menuObject;
    public GameObject loadingScreenObject;
    public float delay = 3f; // seconds to wait before swap

    public TextMeshProUGUI countdownText;





    public void StartGame()
    {

        loadingScreenObject.SetActive(true);
        menuObject.SetActive(false);
        StartCoroutine(SwapObjectsAfterDelay());

    }
    private IEnumerator SwapObjectsAfterDelay()
    {
        float timeLeft = delay;

        while (timeLeft > 0f)
        {
            if (countdownText != null)
                countdownText.text = Mathf.Ceil(timeLeft).ToString();

            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        if (countdownText != null)
            countdownText.text = "0";

        if (enemyFactory != null)
            enemyFactory.SetActive(true);

        if (loadingScreenObject != null)
            loadingScreenObject.SetActive(false);
    }

    public void Quit()
    {

        Application.Quit();

    }
}
