using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
public class SpearCenterOfMass : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        //grabInteractable.selectExited.AddListener(OnRelease);
    }

    /*void OnRelease(SelectExitEventArgs args)
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            // 1. Look in the throw direction
            Quaternion velocityRotation = Quaternion.LookRotation(rb.linearVelocity, Vector3.up);

            // 2. Decompose velocityRotation into Euler angles (X, Y, Z)
            Vector3 throwEuler = velocityRotation.eulerAngles;

            // 3. Fix the X rotation to 11.3395624f, but keep the Y and Z from the throw
            Vector3 finalRotation = new Vector3(throwEuler.x, throwEuler.y, throwEuler.z);

            // 4. Apply the final rotation to the spear
            transform.eulerAngles = finalRotation;

            Debug.Log($"Spear Released with Dynamic Y/Z, Fixed X. Final Rotation: {finalRotation}");
        }
    }
    */
    void OnDrawGizmos()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // Set gizmo color
        Gizmos.color = Color.red;

        // Calculate world position of center of mass
        Vector3 worldCenterOfMass = transform.TransformPoint(rb.centerOfMass);

        // Draw a small sphere at the center of mass
        Gizmos.DrawSphere(worldCenterOfMass, 0.02f);  // 0.02 = small sphere size
    }
}

/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
public class SpearCenterOfMass : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            // Set the spear to a fixed rotation when released
            transform.eulerAngles = new Vector3(11.3395624f, 262.872437f, 269.735718f);

            Debug.Log("Spear Released with Fixed Rotation");
        }
    }
}*/