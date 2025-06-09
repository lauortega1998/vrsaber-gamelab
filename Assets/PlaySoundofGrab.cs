using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlaySoundOnGrab : MonoBehaviour
{
    public string soundName = "grab"; // Replace with your actual sound name
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("[PlaySoundOnGrab] XRGrabInteractable not found on this object.");
            return;
        }

        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        AudioManager audioManager = FindAnyObjectByType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Play(soundName);
            Debug.LogWarning("grab played lol");
        }
        else
        {
            Debug.LogWarning("[PlaySoundOnGrab] AudioManager not found.");
        }
    }
}
