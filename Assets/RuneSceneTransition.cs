using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class NewRuneScript : MonoBehaviour
{
    public string sceneToLoad = "YourSceneNameHere";
    public float delayBeforeLoad = 7f;

    [Header("UI & FX")]
    public Image fadeImage;
    public Volume postProcessVolume;

    [Header("Rune Visuals")]
    public GameObject originalRune;
    public GameObject brokenRune;

    private bool isSmashed = false;
    private Vignette vignette;

    void Start()
    {
        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out Vignette v))
        {
            vignette = v;
            vignette.intensity.Override(0f);
        }

        if (fadeImage != null)
        {
            var col = fadeImage.color;
            col.a = 0f;
            fadeImage.color = col;
        }

        if (brokenRune != null) brokenRune.SetActive(false);
    }

    // This method is called externally
    public void BreakRune()
    {
        if (isSmashed) return;
        StartCoroutine(HandleSmash());
    }

    private IEnumerator HandleSmash()
    {
        FindAnyObjectByType<AudioManager>().Play("stone break");
        FindAnyObjectByType<AudioManager>().Play("Horn2");

        isSmashed = true;
        Debug.Log("Smash registered – transitioning scene...");

        if (originalRune != null) originalRune.SetActive(false);
        if (brokenRune != null) brokenRune.SetActive(true);

        float elapsed = 0f;

        while (elapsed < delayBeforeLoad)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / delayBeforeLoad;

            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                fadeImage.color = color;
            }

            if (vignette != null)
            {
                vignette.intensity.Override(Mathf.Lerp(0f, 0.5f, t));
            }

            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
