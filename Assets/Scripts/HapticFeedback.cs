using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class HapticFeedback : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand; // Choose LeftHand if needed
    public float intensity = 0.5f;  // Haptic intensity (range 0 to 1)
    public float duration = 0.1f;   // Duration of the haptic feedback

    private InputDevice inputDevice;

    void Start()
    {
        // Initialize the InputDevice
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

    public void TriggerHapticFeedback()
    {
        if (inputDevice.isValid)
        {
            inputDevice.SendHapticImpulse(0, intensity, duration); // Channel 0, intensity, duration
            Debug.Log("Triggered Haptic Feedback.");
        }
        else
        {
            Debug.LogWarning("Input Device not valid, cannot trigger haptic feedback.");
        }
    }

    public void DestroyWithFeedback()
    {
        // Trigger haptic feedback first
        TriggerHapticFeedback();

        // Then destroy the object
       // Destroy(gameObject);
    }
}