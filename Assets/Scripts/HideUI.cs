using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideUIOnGripBothHands : MonoBehaviour
{
    [Header("UI Elements to Hide")]
    public GameObject leftHandUI;
    public GameObject rightHandUI;
    public List<GameObject> pickupIndicator;  
    [Header("Grip Input Actions")]
    public InputActionReference leftGripAction;  // Assign Left Grip action
    public InputActionReference rightGripAction; // Assign Right Grip action

    private void OnEnable()
    {
        leftGripAction.action.performed += OnLeftGripPressed;
        rightGripAction.action.performed += OnRightGripPressed;
        leftGripAction.action.canceled += OnLeftGripPressed;
        rightGripAction.action.canceled += OnRightGripPressed;



        leftGripAction.action.Enable();
        rightGripAction.action.Enable();
    }

    private void OnDisable()
    {
        leftGripAction.action.performed -= OnLeftGripPressed;
        rightGripAction.action.performed -= OnRightGripPressed;
        leftGripAction.action.canceled -= OnLeftGripPressed;
        rightGripAction.action.canceled -= OnRightGripPressed;

        leftGripAction.action.Disable();
        rightGripAction.action.Disable();
    }

    private void OnLeftGripPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (leftHandUI != null)
                leftHandUI.SetActive(false);

            foreach (GameObject obj in pickupIndicator)
            {
                obj.SetActive(false);
            }
        }
        else if (context.canceled)
        {
           

            foreach (GameObject obj in pickupIndicator)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnRightGripPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (rightHandUI != null)
                rightHandUI.SetActive(false);

            foreach (GameObject obj in pickupIndicator)
            {
                obj.SetActive(false);
            }
        }
        else if (context.canceled)
        {
           

            foreach (GameObject obj in pickupIndicator)
            {
                obj.SetActive(true);
            }
        }
    }
}
