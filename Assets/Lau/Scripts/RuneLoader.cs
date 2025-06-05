using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class RuneTeleporter : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad;
    public float delayBeforeLoad = 7f;

    [Header("Visuals & Effects")]
    public Image fadeImage;
    public Volume postProcessVolume;
    public GameObject originalRune;
    public GameObject brokenRune;

    private bool hasTriggered = false;
    private Vignette vignette;

    private void Start()
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

    public void BreakRune()
    {
        if (hasTriggered || RuneSmashManager.runeAlreadySmashed) return;

        hasTriggered = true;
        RuneSmashManager.runeAlreadySmashed = true;

        StartCoroutine(DoTransition());
    }

    private IEnumerator DoTransition()
    {
        Debug.Log($"[RuneTeleporter] Rune smashed, preparing to load {sceneToLoad}");

        FindAnyObjectByType<AudioManager>()?.Play("stone break");
        FindAnyObjectByType<AudioManager>()?.Play("Horn2");

        if (originalRune) originalRune.SetActive(false);
        if (brokenRune) brokenRune.SetActive(true);

        float elapsed = 0f;

        while (elapsed < delayBeforeLoad)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / delayBeforeLoad;

            if (fadeImage != null)
            {
                var col = fadeImage.color;
                col.a = Mathf.Lerp(0f, 1f, t);
                fadeImage.color = col;
            }

            if (vignette != null)
            {
                vignette.intensity.Override(Mathf.Lerp(0f, 0.5f, t));
            }

            yield return null;
        }

        if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            Debug.Log($"[RuneTeleporter] Loading scene {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError($"[RuneTeleporter] Scene '{sceneToLoad}' is NOT in build settings!");
        }
    }
}
