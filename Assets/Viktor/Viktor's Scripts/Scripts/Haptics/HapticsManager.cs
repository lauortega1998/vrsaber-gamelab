using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
  //vibration intensity and amplitude parameters can only be modified in the scripts where haptics are called
  
public class HapticsManager : MonoBehaviour
{
    public static HapticsManager Instance { get; private set; } // a singleton instance, all scripts can call this instance to trigger the haptics

    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;

    private void Awake() //this is to keep one haprics manager and cache connected controllers 
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDevices();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDevices()
    {
        // Find and cache the left controller
        var leftDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftDevices);
        if (leftDevices.Count > 0)
            leftHandDevice = leftDevices[0];

        // Find and cache the righ  controller
        var rightDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightDevices);
        if (rightDevices.Count > 0)
            rightHandDevice = rightDevices[0];
       
       Debug.Log($"[HapticsManager] Left valid:{leftHandDevice.isValid}  Right valid:{rightHandDevice.isValid}");
    }

   // api, simple vibration 
   
    public void TriggerHaptics(float amplitude, float duration, bool left = true, bool right = true)
    {
        if (left && leftHandDevice.isValid)
            leftHandDevice.SendHapticImpulse(0u, amplitude, duration);

        if (right && rightHandDevice.isValid)
            rightHandDevice.SendHapticImpulse(0u, amplitude, duration);
    }

    // re-query devices at runtime if controllers reconnect
    private void OnEnable()
    {
        InputDevices.deviceConnected += OnDeviceConnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
    }

        private void OnDeviceConnected(InputDevice device)
    {
        // Re-initialize if a new controller is connected
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeldInHand))
            InitializeDevices();
    }
}
