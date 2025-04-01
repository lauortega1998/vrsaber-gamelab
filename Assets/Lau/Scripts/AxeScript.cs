/*using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AxeWeapon : MonoBehaviour
{
    public ParticleSystem hitEffect; // Drag your child HitEffect here
    public GameObject hitEffectPrefab; // Assign in Inspector

    public float knockbackForce = 500f;
    public float enemyDestroyDelay = 2f;

    [Header("Haptic Feedback Settings")]
    public float hapticIntensity = 1.0f;
    public float hapticDuration = 0.2f;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject enemyRoot = other.transform.root.gameObject;
        if (enemyRoot.CompareTag("Enemy"))
        {
            Debug.Log("Axe hit an enemy!");

            if (hitEffectPrefab != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                GameObject effect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
                Destroy(effect, 2f); // Destroy after 2 seconds
            }
            // Spawn hit effect
            /*if (hitEffect != null)
            {
                hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Reset it
                hitEffect.Play(); // Play effect at current position
            }

            // Trigger haptics
            TriggerHapticFeedback();

            // Find and switch between enemy states
            Transform normalModel = enemyRoot.transform.Find("NormalModel");
            Transform destroyedModel = enemyRoot.transform.Find("DestroyedModel");

            if (normalModel != null && destroyedModel != null)
            {
                normalModel.gameObject.SetActive(false);
                destroyedModel.gameObject.SetActive(true);

                // Push ragdoll parts
                Rigidbody[] ragdollRigidbodies = destroyedModel.GetComponentsInChildren<Rigidbody>();
                Vector3 pushDirection = Vector3.forward; // Change this if needed

                foreach (Rigidbody rb in ragdollRigidbodies)
                {
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.AddForce(pushDirection * knockbackForce, ForceMode.Impulse);
                    }
                }

                // Destroy the full enemy GameObject after delay
                Destroy(enemyRoot, enemyDestroyDelay);
            }
            else
            {
                Debug.LogWarning("NormalModel or DestroyedModel not found on enemy.");
            }
        }
    }

    private void TriggerHapticFeedback()
    {
        if (grabInteractable == null) return;

        IXRSelectInteractor interactorInterface = grabInteractable.GetOldestInteractorSelecting();
        if (interactorInterface is XRBaseInteractor interactor)
        {
            if (interactor is XRBaseInputInteractor controllerInteractor)
            {
                controllerInteractor.SendHapticImpulse(hapticIntensity, hapticDuration);
            }
        }
    }
}*/