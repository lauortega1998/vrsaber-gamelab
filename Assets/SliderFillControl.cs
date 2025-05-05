using UnityEngine;
using UnityEngine.UI;

public class SliderFillControl : MonoBehaviour
{
    public Slider slider; // Reference to the Slider
    public Image fillImage; // Reference to the Image that will fill
    public float fillSpeed = 0.5f; // Speed of the fill transition

    void Start()
    {
        // Initialize fill image to match slider value at the start
        fillImage.fillAmount = slider.value;

        // Add listener to detect value changes in the slider
        slider.onValueChanged.AddListener(UpdateFill);
    }

    void UpdateFill(float value)
    {
        // Smoothly fill the image based on slider value
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, value, Time.deltaTime * fillSpeed);
    }
}
