using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class HapticFeedback : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand;  // Choose LeftHand if needed
    public float intensity = 0.5f;  // Haptic intensity (range 0 to 1)
    public float duration = 0.1f;   // Duration of the haptic feedback

    private InputDevice inputDevice;

    void Start()
    {
        // Initialize the InputDevice for the selected controller (RightHand or LeftHand)
        inputDevice = GetInputDevice();
        Debug.Log("Input Device: " + inputDevice.name + " valid: " + inputDevice.isValid);
    }

    private InputDevice GetInputDevice()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);
        if (devices.Count > 0)
            return devices[0];

        return new InputDevice(); // returns an invalid device if none is found
    }

    void OnDestroy()
    {
        // Trigger haptic feedback when this object is destroyed
        TriggerHapticFeedback();
    }

    private void TriggerHapticFeedback()
    {
        if (inputDevice.isValid)
        {
            // Send haptic feedback on the selected controller
            inputDevice.SendHapticImpulse(0, intensity, duration); // Channel 0, intensity, duration
            Debug.Log("Haptic feedback triggered on " + controllerNode.ToString());
        }
        else
        {
            Debug.LogWarning("Input Device not valid, cannot trigger haptic feedback.");
        }
    }
}