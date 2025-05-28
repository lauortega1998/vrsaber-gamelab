using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class NewRuneScript : MonoBehaviour
{
    public string sceneToLoad = "YourSceneNameHere"; // Change this in Inspector or here
    public float speedThreshold = 2f;
    public float delayBeforeLoad = 7f;

    [Header("UI & FX")]
    public Image fadeImage; // Fullscreen black Image, alpha = 0 initially
    public Volume postProcessVolume; // Global Volume with Vignette override

    private bool isSmashed = false;
    private Vignette vignette;

    void Start()
    {
        // Get Vignette reference from Post Processing Volume
        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out Vignette v))
        {
            vignette = v;
            vignette.intensity.Override(0f);
        }

        // Make sure fadeImage starts fully transparent
        if (fadeImage != null)
        {
            var col = fadeImage.color;
            col.a = 0f;
            fadeImage.color = col;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSmashed) return;

        if (other.CompareTag("Weapon") && other.attachedRigidbody != null)
        {
            float impactSpeed = other.attachedRigidbody.linearVelocity.magnitude;
            if (impactSpeed >= speedThreshold)
            {
                StartCoroutine(HandleSmash());
            }
        }
    }

    private IEnumerator HandleSmash()
    {
        isSmashed = true;
        Debug.Log("Smash registered � transitioning scene...");

        float elapsed = 0f;

        while (elapsed < delayBeforeLoad)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / delayBeforeLoad;

            // Fade screen to black
            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                fadeImage.color = color;
            }

            // Increase vignette intensity
            if (vignette != null)
            {
                vignette.intensity.Override(Mathf.Lerp(0f, 0.5f, t));
            }

            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
