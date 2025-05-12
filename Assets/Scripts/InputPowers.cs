using UnityEngine;
using UnityEngine.InputSystem;

public class InputPowers : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference leftTriggerAction;  // Fire Power (Left Trigger)
    public InputActionReference rightTriggerAction; // Ice Power (Right Trigger)

    [Header("Power Objects")]
    public GameObject firePowerObject; // Object for Fire Power
    public GameObject icePowerObject;  // Object for Ice Power

    public GameObject firePowerTutorial;
    public GameObject icePowerTutorial;
    
    public PlayerStats playerStats; // Reference to the PlayerStats script

    void OnEnable()
    {
        leftTriggerAction.action.Enable();
        rightTriggerAction.action.Enable();
        playerStats = GetComponent<PlayerStats>();
    }

    void OnDisable()
    {
        leftTriggerAction.action.Disable();
        rightTriggerAction.action.Disable();
    }

    void Update()
    {
        if (playerStats != null)
        {
            // Fire Power Activation (Left Trigger)
            if (leftTriggerAction.action.ReadValue<float>() > 0.1f && playerStats.CurrentMana > 0)
            {
                firePowerObject.SetActive(true);
                firePowerTutorial.SetActive(false);
            }
            else
            {
                firePowerObject.SetActive(false);
            }

            // Ice Power Activation (Right Trigger)
            if (rightTriggerAction.action.ReadValue<float>() > 0.1f && playerStats.CurrentMana > 0)
            {
                icePowerObject.SetActive(true);
                icePowerTutorial.SetActive(false);
            }
            else
            {
                icePowerObject.SetActive(false);
            }
        }
    }
}
