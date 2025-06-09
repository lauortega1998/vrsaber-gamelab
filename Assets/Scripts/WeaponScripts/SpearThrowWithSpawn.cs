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
    public float throwingSpeedThreshold = 15f; // Lowered for build testing
    public GameObject spearPrefab;
    public GameObject respawnPrefab;
    public XRRayInteractor gazeInteractor;
    public float throwForce = 10f;

    private bool isHeld = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 cachedVelocity = Vector3.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isHeld && rb != null)
        {
            cachedVelocity = (transform.position - originalPosition) / Time.deltaTime;
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;

        float throwSpeed = cachedVelocity.magnitude;
        Debug.Log($"[THROW] Spear released with tracked speed: {throwSpeed:F2} m/s");

        if (throwSpeed >= throwingSpeedThreshold)
        {
            Debug.Log("[THROW] Speed high enough! Spear thrown.");
            FindAnyObjectByType<AudioManager>()?.Play("spear");

            Vector3 throwDir = GetGazeDirection();
            Quaternion spawnRotation = Quaternion.LookRotation(throwDir, Vector3.up);
            GameObject thrownSpear = Instantiate(spearPrefab, transform.position, spawnRotation);

            Rigidbody thrownRb = thrownSpear.GetComponent<Rigidbody>();
            if (thrownRb != null)
            {
                thrownRb.linearVelocity = throwDir * throwForce;
            }

            Destroy(thrownSpear, 6f);

            if (respawnPrefab != null)
                Instantiate(respawnPrefab, originalPosition, originalRotation);
            else
                Debug.LogWarning("Respawn prefab is not assigned!");

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[THROW] Too slow. Spear not thrown.");
        }
    }

    Vector3 GetGazeDirection()
    {
        if (gazeInteractor != null)
            return gazeInteractor.transform.forward.normalized;
        else
        {
            Debug.LogWarning("No gaze interactor found. Using transform.forward.");
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



/*
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
            SpearRespawner respawner = GetComponent<SpearRespawner>();
            if (respawner != null)
            {
                respawner.MarkAsThrown();
            }
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
*/
