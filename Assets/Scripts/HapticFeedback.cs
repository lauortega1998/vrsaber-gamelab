using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic; // For List

public class HapticFeedback : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand; // Change to LeftHand if needed
    public float intensity = 0.5f;  // Haptic intensity (range 0 to 1)
    public float duration = 0.1f;   // Duration of the haptic feedback

    private InputDevice inputDevice;

    private void Start()
    {
        // Get the InputDevice for the specified hand (left or right)
        inputDevice = GetInputDevice();
    }

    private void OnDestroy()
    {
        // Trigger haptic feedback when this object is destroyed
        if (inputDevice.isValid)
        {
            TriggerHapticFeedback(intensity, duration);
        }
    }

    private InputDevice GetInputDevice()
    {
        // Retrieve the InputDevice based on the selected controller node (left or right hand)
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);

        if (devices.Count > 0)
        {
            return devices[0];
        }

        return new InputDevice(); // Return an invalid InputDevice if none found
    }

    private void TriggerHapticFeedback(float intensity, float duration)
    {
        if (inputDevice.isValid)
        {
            inputDevice.SendHapticImpulse(0, intensity, duration); // Channel 0, intensity, duration
        }
    }
}