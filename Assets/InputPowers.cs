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

    void OnEnable()
    {
        leftTriggerAction.action.Enable();
        rightTriggerAction.action.Enable();
    }

    void OnDisable()
    {
        leftTriggerAction.action.Disable();
        rightTriggerAction.action.Disable();
    }

    void Update()
    {
        // Fire Power Activation (Left Trigger)
        if (leftTriggerAction.action.ReadValue<float>() > 0.1f)
        {
            firePowerObject.SetActive(true);
        }
        else
        {
            firePowerObject.SetActive(false);
        }

        // Ice Power Activation (Right Trigger)
        if (rightTriggerAction.action.ReadValue<float>() > 0.1f)
        {
            icePowerObject.SetActive(true);
        }
        else
        {
            icePowerObject.SetActive(false);
        }
    }
}
