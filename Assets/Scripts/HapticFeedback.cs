using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;


public class HapticFeedback : MonoBehaviour
{
    [SerializeField] private XRNode controllerNode = XRNode.RightHand; // or XRNode.LeftHand
    [SerializeField] private float amplitude = 0.7f;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private string enemyTag = "Enemy";

    private InputDevice device;

    private void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(controllerNode);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            TriggerHaptic();
        }
    }

    private void TriggerHaptic()
    {
        if (device.isValid)
        {
            device.SendHapticImpulse(0u, amplitude, duration);
        }
        else
        {
            Debug.LogWarning("Haptic device not valid.");
        }
    }
}