using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SpearGrabCrosshair : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    [SerializeField] private GameObject crosshairObject; 

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (crosshairObject != null)
        {
            crosshairObject.SetActive(false); 
        }
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (crosshairObject != null)
        {
            crosshairObject.SetActive(true);
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (crosshairObject != null)
        {
            crosshairObject.SetActive(false);
        }
    }
}