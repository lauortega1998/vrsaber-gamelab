using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TwoHandedGrab : XRGrabInteractable
{
    [Header("Second Hand")]
    public Transform secondHandGrabPoint;

    private XRBaseInteractor firstHandInteractor;
    private XRBaseInteractor secondHandInteractor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        firstHandInteractor = args.interactorObject as XRBaseInteractor;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject == secondHandInteractor)
        {
            secondHandInteractor = null;
        }
        else if (args.interactorObject == firstHandInteractor)
        {
            firstHandInteractor = null;

            // promote second hand to main if still holding
            if (secondHandInteractor != null)
            {
                firstHandInteractor = secondHandInteractor;
                secondHandInteractor = null;
            }
        }

        base.OnSelectExited(args);
    }

    public void GrabSecondHand(XRBaseInteractor interactor)
    {
        if (secondHandInteractor == null)
        {
            secondHandInteractor = interactor;
        }
    }

    public void ReleaseSecondHand(XRBaseInteractor interactor)
    {
        if (interactor == secondHandInteractor)
        {
            secondHandInteractor = null;
        }
    }

    void Update()
    {
        if (firstHandInteractor != null && secondHandInteractor != null)
        {
            Vector3 direction = secondHandInteractor.transform.position - firstHandInteractor.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // rotate the object between the two hands
            transform.rotation = targetRotation;
            transform.position = firstHandInteractor.transform.position;
        }
    }
}