using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeaponRespawner : MonoBehaviour
{
    public float respawnTime = 10f;  // Time before respawn
    private float timer = 0f;        // Current time left for respawn
    public bool isDropped = false;  // Flag to check if the weapon is dropped
    private Vector3 originalPosition; // Original position to teleport the weapon back
    private Quaternion originalRotation; // Original rotation of the weapon

    private XRGrabInteractable grabInteractable; // Reference to the XR grab interactable component

    void Start()
    {
        originalPosition = transform.position;  // Store the original position
        originalRotation = transform.rotation;  // Store the original rotation
        grabInteractable = GetComponent<XRGrabInteractable>();  // Get the grab interactable component

        grabInteractable.selectExited.AddListener(OnWeaponDropped);  // Listen for weapon drop
    }

    void Update()
    {
        if (isDropped)
        {
            timer -= Time.deltaTime;  // Decrease the timer while the weapon is dropped

            if (timer <= 0f)
            {
                RespawnWeapon();  // Respawn the weapon when timer runs out
            }
        }
    }

    // Called when the player drops the weapon
    void OnWeaponDropped(SelectExitEventArgs args)
    {
        if (!isDropped && !args.isCanceled)  // Check if the weapon wasn't canceled
        {
            isDropped = true;
            timer = respawnTime;  // Start the timer when the weapon is dropped
        }
    }

    // Called when the player picks up the weapon again
    public void OnWeaponPickedUp()
    {
        isDropped = false;  // Stop the timer when the weapon is picked up
        timer = 0f;
    }

    // Respawn the weapon at its original position
    void RespawnWeapon()
    {
        transform.position = originalPosition;  // Teleport weapon to original position
        transform.rotation = originalRotation;  // Reset weapon rotation
        isDropped = false;  // Reset drop flag
    }
}
