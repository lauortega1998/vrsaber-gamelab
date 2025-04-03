using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ParryMechanic : MonoBehaviour
{
    public XRGrabInteractable objectLeft; // Assign the left object
    public XRGrabInteractable objectRight; // Assign the right object
    public Transform playerHead; // Assign the XR Rig's head for reference
    public Transform parryPosition; // Position where objects should move

    private XRBaseInteractor leftHand;
    private XRBaseInteractor rightHand;

    void Start()
    {
        objectLeft.selectEntered.AddListener(ctx => leftHand = ctx.interactorObject as XRBaseInteractor);
        objectLeft.selectExited.AddListener(ctx => leftHand = null);

        objectRight.selectEntered.AddListener(ctx => rightHand = ctx.interactorObject as XRBaseInteractor);
        objectRight.selectExited.AddListener(ctx => rightHand = null);
    }

    void Update()
    {
        if (leftHand != null && rightHand != null) // Both objects are held
        {
            //if (XRinputs.GetDown(OVRInput.Button.One)) // Replace with your button
            //{
            //    ActivateParry();
           // }
        }
    }

    void ActivateParry()
    {
        if (parryPosition == null)
        {
            Debug.LogWarning("Parry position is not set!");
            return;
        }

        objectLeft.transform.position = parryPosition.position + parryPosition.right * -0.1f; // Slightly left
        objectRight.transform.position = parryPosition.position + parryPosition.right * 0.1f; // Slightly right

        objectLeft.transform.rotation = parryPosition.rotation;
        objectRight.transform.rotation = parryPosition.rotation;
    }
}