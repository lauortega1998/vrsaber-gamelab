using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SecondaryGrabTrigger : MonoBehaviour
{
    public TwoHandedGrab mainTwoHandedScript;

    private void OnTriggerEnter(Collider other)
    {
        XRBaseInteractor interactor = other.GetComponent<XRBaseInteractor>();
        if (interactor != null)
        {
            mainTwoHandedScript.GrabSecondHand(interactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        XRBaseInteractor interactor = other.GetComponent<XRBaseInteractor>();
        if (interactor != null)
        {
            mainTwoHandedScript.ReleaseSecondHand(interactor);
        }
    }
}