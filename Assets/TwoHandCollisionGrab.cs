using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TwoHandedCollisionGrab : MonoBehaviour
{
    [Header("Grab Zones")]
    public Collider grabZoneA;
    public Collider grabZoneB;

    private int zoneAHandsTouching = 0;
    private int zoneBHandsTouching = 0;

    private bool isGrabbed = false;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        }

        grabInteractable.enabled = false;

        // Setup sub-collider triggers
        SetupTriggerZone(grabZoneA, OnZoneATriggerEnter, OnZoneATriggerExit);
        SetupTriggerZone(grabZoneB, OnZoneBTriggerEnter, OnZoneBTriggerExit);
    }

    private void SetupTriggerZone(Collider collider,
        System.Action<Collider> onEnter,
        System.Action<Collider> onExit)
    {
        var trigger = collider.gameObject.AddComponent<GrabZoneTrigger>();
        trigger.onEnter = onEnter;
        trigger.onExit = onExit;
    }

    private void OnZoneATriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            zoneAHandsTouching++;
            TryEnableGrab();
        }
    }

    private void OnZoneATriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            zoneAHandsTouching = Mathf.Max(0, zoneAHandsTouching - 1);
            TryDisableGrab();
        }
    }

    private void OnZoneBTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            zoneBHandsTouching++;
            TryEnableGrab();
        }
    }

    private void OnZoneBTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            zoneBHandsTouching = Mathf.Max(0, zoneBHandsTouching - 1);
            TryDisableGrab();
        }
    }

    private void TryEnableGrab()
    {
        if (!isGrabbed && zoneAHandsTouching > 0 && zoneBHandsTouching > 0)
        {
            grabInteractable.enabled = true;
        }
    }

    private void TryDisableGrab()
    {
        if (!isGrabbed && (zoneAHandsTouching == 0 || zoneBHandsTouching == 0))
        {
            grabInteractable.enabled = false;
        }
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;

        // Recheck if we still have both hands touching
        if (zoneAHandsTouching == 0 || zoneBHandsTouching == 0)
        {
            grabInteractable.enabled = false;
        }
    }
}
