using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HoverUI : MonoBehaviour
{
    public GameObject uiElement; // UI element to show when hovered

    private XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();

        if (uiElement != null)
            uiElement.SetActive(false); // Make sure it's hidden by default

        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnHoverEnter);
            interactable.hoverExited.AddListener(OnHoverExit);
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEnter);
            interactable.hoverExited.RemoveListener(OnHoverExit);
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (uiElement != null)
            uiElement.SetActive(true);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        if (uiElement != null)
            uiElement.SetActive(false);
    }
}
