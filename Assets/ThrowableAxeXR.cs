using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ThrowableAxeXR : MonoBehaviour
{
    public float throwForce = 10f;  // The force at which the axe is thrown
    public float rotationSpeed = 500f; // The speed at which the axe rotates

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable; // Reference to XR Grab Interactable
    private bool isThrown = false; // Flag to check if the axe is thrown

    void Start()
    {
        // Get references to the Rigidbody and XR Grab Interactable components
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Listen for the select exit event when the axe is thrown or dropped
        grabInteractable.selectExited.AddListener(OnAxeThrown);
    }

    // This is called when the player throws the axe
    private void OnAxeThrown(SelectExitEventArgs args)
    {
        if (!isThrown)
        {
            isThrown = true;
            Vector3 throwDirection = (transform.position - args.interactorObject.transform.position).normalized;
            Throw(throwDirection); // Call Throw with the direction based on the player's hand position
        }
    }

    // Apply force to throw the axe
    public void Throw(Vector3 direction)
    {
        rb.isKinematic = false; // Enable physics interaction when thrown
        rb.AddForce(direction * throwForce, ForceMode.VelocityChange); // Apply throw force
    }

    void Update()
    {
        if (isThrown && rb.linearVelocity.magnitude > 0.1f) // If the axe is moving
        {
            // Rotate the axe continuously as it moves through the air
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    // Reset rotation and kinematic state when axe is picked up again
    public void OnAxePickedUp()
    {
        isThrown = false;
        rb.isKinematic = true;  // Disable physics when the axe is grabbed again
    }
}