using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light lightComponent;  // Reference to the light component
    public Light otherLight;  // Reference to the other light that will be deactivated
    public AudioSource audioSource;  // Reference to the AudioSource
    public AudioClip thunderSound;  // Thunder sound clip
    public float activeDuration = 5f;  // Duration the light stays active
    public float inactiveDuration = 3f;  // Duration the light stays inactive
    public float minFlickerInterval = 0.05f;  // Minimum flicker interval (in seconds)
    public float maxFlickerInterval = 0.2f;  // Maximum flicker interval (in seconds)
    public float flickerIntensityMin = 0f;  // Minimum intensity when flickering
    public float flickerIntensityMax = 10f;  // Maximum intensity when flickering

    private bool isActive = false;  // Whether the light is currently active
    private float timer = 0f;  // Timer for controlling light activation and deactivation
    private float flickerTimer = 0f;  // Timer for flicker intervals

    private bool hasRotated = false;  // To track if the rotation has been set for this activation
    private bool hasPlayedThunderSound = false;  // To track if thunder sound has been played

    void Start()
    {

        lightComponent = GetComponent<Light>();  // Get the Light component attached to the GameObject
        if (lightComponent == null)
        {
            Debug.LogError("No Light component found on this GameObject!");
            return;
        }

        lightComponent.enabled = false;  // Start with the light off
        if (otherLight != null)
        {
            otherLight.enabled = true;  // Ensure the other light starts as enabled
        }
        timer = inactiveDuration;  // Set initial timer for deactivation

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();  // Ensure AudioSource is attached
        }


    }

    void Update()
    {
        if (isActive)
        {
            // If the light is active, start the flickering process
            FlickerLight();
            FindAnyObjectByType<AudioManager>().Play("Thunder2");
            flickerTimer -= Time.deltaTime;

            // If the flicker timer has elapsed, reset the flicker with a random interval
            if (flickerTimer <= 0f)
            {
                flickerTimer = Random.Range(minFlickerInterval, maxFlickerInterval);
                lightComponent.intensity = Random.Range(flickerIntensityMin, flickerIntensityMax);
            }

            // Decrease the timer for the active light duration
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                // Deactivate the light after the active duration
                isActive = false;
                lightComponent.enabled = false;

                // Reactivate the other light
                if (otherLight != null)
                {
                    otherLight.enabled = true;
                }

                timer = inactiveDuration;  // Reset the timer for the inactive duration
                hasRotated = false;  // Reset the rotation flag for the next activation
                hasPlayedThunderSound = false;  // Reset the thunder sound flag
            }
        }
        else
        {
            // If the light is inactive, decrease the timer for the inactive period
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                // Activate the light after the inactive duration
                isActive = true;
                lightComponent.enabled = true;

                // Deactivate the other light while this one is active
                if (otherLight != null)
                {
                    otherLight.enabled = false;
                }

                // Play the thunder sound only once when the light is activated
                if (!hasPlayedThunderSound && thunderSound != null)
                {
                    FindAnyObjectByType<AudioManager>().Play("Thunder" + Random.Range(1, 3));
                    hasPlayedThunderSound = true;  // Set flag to prevent it from playing again
                }

                // Randomize the rotation once when the light is activated
                //if (!hasRotated)
                {
                    //  RandomizeRotation();
                    hasRotated = true;  // Set the flag to ensure rotation only happens once per activation
                }

                timer = activeDuration;  // Reset the timer for the active duration
            }
        }
    }

    // This method controls the flickering effect while the light is active
    private void FlickerLight()
    {
        // You can add custom logic for more complex flickering if needed.
        // For now, it's handled by the timer and random intensity values.
    }

    // Randomizes the rotation of the light at the beginning of each activation
    private void RandomizeRotation()
    {
        float randomRotationX = Random.Range(0f, 360f);
        float randomRotationY = Random.Range(0f, 360f);
        float randomRotationZ = Random.Range(0f, 360f);

        // Apply the random rotation
        transform.rotation = Quaternion.Euler(randomRotationX, randomRotationY, randomRotationZ);
    }
}