
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(Rigidbody))]
public class SpearThrowWithSpawn : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    [Header("Throw Settings")]
    public float throwingSpeedThreshold = 25f; // <-- Real-world speed in m/s
    public GameObject spearPrefab;
    public XRRayInteractor gazeInteractor; // Reference to the XR Interactor for gaze

    [Header("Throw Force")]
    public float throwForce = 10f; // How fast the spear should fly based on gaze

    private bool isHeld = false; // To track when spear is grabbed

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;

        float throwSpeed = rb.linearVelocity.magnitude;
        Debug.Log($"[THROW] Spear released with speed: {throwSpeed:F2} m/s");

        if (throwSpeed > throwingSpeedThreshold)
        {
            Debug.Log("[THROW] Speed high enough! Spawning new spear.");

            // 1. Calculate gaze direction
            Vector3 gazeDirection = GetGazeDirection();

            // 2. Create a rotation that looks in the gaze direction
            Quaternion spawnRotation = Quaternion.LookRotation(gazeDirection, Vector3.up);

            // 3. Spawn the spear with this rotation
            GameObject thrownSpear = Instantiate(spearPrefab, transform.position, spawnRotation);

            Debug.Log($"[THROW] Spawned Spear Rotation (Euler Angles): {thrownSpear.transform.eulerAngles}");

            // 4. Apply velocity based on gaze
            Rigidbody thrownRb = thrownSpear.GetComponent<Rigidbody>();
            if (thrownRb != null)
            {
                thrownRb.linearVelocity = gazeDirection * throwForce;
            }

            //  NEW: Destroy the thrown spear after 6 seconds
            Destroy(thrownSpear, 6f);

            // 5. Destroy the original spear immediately
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[THROW] Speed too low. No spear thrown.");
            // Don't destroy the held spear
        }
    }

    void Update()
    {
        if (isHeld && rb != null)
        {
            float currentSpeed = rb.linearVelocity.magnitude;
//            Debug.Log($"[UPDATE] Current spear speed: {currentSpeed:F2} m/s");
        }
    }

    Vector3 GetGazeDirection()
    {
        if (gazeInteractor != null)
        {
            // The XR Ray Interactor points a ray; we get its forward direction
            return gazeInteractor.transform.forward.normalized;
        }
        else
        {
            Debug.LogWarning("No gaze interactor assigned! Using default forward direction.");
            return transform.forward;
        }
    }

    void OnDrawGizmos()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        Gizmos.color = Color.red;
        Vector3 worldCenterOfMass = transform.TransformPoint(rb.centerOfMass);
        Gizmos.DrawSphere(worldCenterOfMass, 0.02f);
    }
}

/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(Rigidbody))]
public class SpearThrowWithSpawn : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    [Header("Throw Settings")]
    public float throwingVelocityThreshold = 1.5f;
    public GameObject spearPrefab;
    public XRRayInteractor gazeInteractor; // Reference to the XR Interactor for gaze

    [Header("Throw Force")]
    public float throwForce = 10f; // How fast the spear should fly based on gaze

    [Header("Spear Spawn Rotation")]
    public Vector3 spearSpawnRotation = new Vector3(11.3395624f, 262.872437f, 269.735718f);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (rb.linearVelocity.magnitude > throwingVelocityThreshold)
        {
            Debug.Log("Throw velocity high enough, spawning new spear based on gaze!");

            // 1. Spawn a new spear
            Quaternion spawnRotation = Quaternion.Euler(spearSpawnRotation);
            GameObject thrownSpear = Instantiate(spearPrefab, transform.position, spawnRotation);

            Debug.Log($"Spawned Spear Rotation (Euler Angles): {thrownSpear.transform.eulerAngles}");

            // 2. Calculate gaze direction
            Vector3 gazeDirection = GetGazeDirection();

            // 3. Apply velocity based on gaze direction
            Rigidbody thrownRb = thrownSpear.GetComponent<Rigidbody>();
            if (thrownRb != null)
            {
                thrownRb.linearVelocity = gazeDirection * throwForce;
            }

            // 4. Destroy the original spear
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Throw velocity too low, spear not thrown.");
        }
    }


    Vector3 GetGazeDirection()
    {
        if (gazeInteractor != null)
        {
            // The XR Ray Interactor points a ray; we get its forward direction
            return gazeInteractor.transform.forward.normalized;
        }
        else
        {
            Debug.LogWarning("No gaze interactor assigned! Using default forward direction.");
            return transform.forward;
        }
    }

    void OnDrawGizmos()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        Gizmos.color = Color.red;
        Vector3 worldCenterOfMass = transform.TransformPoint(rb.centerOfMass);
        Gizmos.DrawSphere(worldCenterOfMass, 0.02f);
    }
}*/