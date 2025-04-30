using UnityEngine;

public class TorchFlicker : MonoBehaviour
{
    public float minIntensity = 0.5f;  // Minimum light intensity
    public float maxIntensity = 1.5f;  // Maximum light intensity
    public float minRange = 3f;        // Minimum light range
    public float maxRange = 6f;        // Maximum light range
    public float flickerSpeed = 0.1f;  // Speed of flicker effect

    private Light torchLight;  // Reference to the light component
    private float targetIntensity;
    private float targetRange;

    void Start()
    {
        // Get the Light component attached to this GameObject
        torchLight = GetComponent<Light>();

        if (torchLight == null)
        {
            Debug.LogError("No Light component found on this GameObject.");
            return;
        }

        // Set the initial target intensity and range
        targetIntensity = torchLight.intensity;
        targetRange = torchLight.range;
    }

    void Update()
    {
        // Ensure the light component exists
        if (torchLight == null)
            return;

        // Randomly set new target intensity and range for flickering effect
        targetIntensity = Random.Range(minIntensity, maxIntensity);
        targetRange = Random.Range(minRange, maxRange);

        // Smoothly transition to the new intensity and range
        torchLight.intensity = Mathf.Lerp(torchLight.intensity, targetIntensity, flickerSpeed * Time.deltaTime);
        torchLight.range = Mathf.Lerp(torchLight.range, targetRange, flickerSpeed * Time.deltaTime);
    }
}