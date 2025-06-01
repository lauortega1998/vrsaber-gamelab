using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light lightComponent;
    public Light otherLight;

    public float activeDuration = 5f;
    public float inactiveDuration = 3f;
    public float minFlickerInterval = 0.05f;
    public float maxFlickerInterval = 0.2f;
    public float flickerIntensityMin = 0f;
    public float flickerIntensityMax = 10f;

    private bool isActive = false;
    private float timer = 0f;
    private float flickerTimer = 0f;

    private bool hasRotated = false;
    private bool hasPlayedThunderSound = false;
    private bool playThunder1Next = true;

    void Start()
    {
        lightComponent = GetComponent<Light>();
        if (lightComponent == null)
        {
            Debug.LogError("No Light component found on this GameObject!");
            return;
        }

        lightComponent.enabled = false;

        if (otherLight != null)
            otherLight.enabled = true;

        timer = inactiveDuration;
    }

    void Update()
    {
        if (isActive)
        {
            FlickerLight();
            flickerTimer -= Time.deltaTime;

            if (flickerTimer <= 0f)
            {
                flickerTimer = Random.Range(minFlickerInterval, maxFlickerInterval);
                lightComponent.intensity = Random.Range(flickerIntensityMin, flickerIntensityMax);
            }

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isActive = false;
                lightComponent.enabled = false;

                if (otherLight != null)
                    otherLight.enabled = true;

                timer = inactiveDuration;
                hasRotated = false;
                hasPlayedThunderSound = false;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isActive = true;
                lightComponent.enabled = true;

                if (otherLight != null)
                    otherLight.enabled = false;

                // ✅ Alternate thunder playback using AudioManager
                if (!hasPlayedThunderSound)
                {
                    string thunderClip = playThunder1Next ? "Thunder1" : "Thunder2";
                    AudioManager audioManager = FindAnyObjectByType<AudioManager>();
                    if (audioManager != null)
                    {
                        audioManager.Play(thunderClip);
                    }

                    playThunder1Next = !playThunder1Next;
                    hasPlayedThunderSound = true;
                }

                hasRotated = true; // If you ever re-enable random rotation
                timer = activeDuration;
            }
        }
    }

    private void FlickerLight()
    {
        // handled in Update
    }
}
