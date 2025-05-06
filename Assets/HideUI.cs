using UnityEngine;
using UnityEngine.InputSystem;

public class HideUIOnGripBothHands : MonoBehaviour
{
    [Header("UI Elements to Hide")]
    public GameObject leftHandUI;
    public GameObject rightHandUI;

    [Header("Grip Input Actions")]
    public InputActionReference leftGripAction;  // Assign Left Grip action
    public InputActionReference rightGripAction; // Assign Right Grip action

    private void OnEnable()
    {
        leftGripAction.action.performed += OnLeftGripPressed;
        rightGripAction.action.performed += OnRightGripPressed;

        leftGripAction.action.Enable();
        rightGripAction.action.Enable();
    }

    private void OnDisable()
    {
        leftGripAction.action.performed -= OnLeftGripPressed;
        rightGripAction.action.performed -= OnRightGripPressed;

        leftGripAction.action.Disable();
        rightGripAction.action.Disable();
    }

    private void OnLeftGripPressed(InputAction.CallbackContext context)
    {
        if (leftHandUI != null)
            leftHandUI.SetActive(false);
    }

    private void OnRightGripPressed(InputAction.CallbackContext context)
    {
        if (rightHandUI != null)
            rightHandUI.SetActive(false);
    }
}
