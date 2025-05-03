using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerObjectActivator : MonoBehaviour
{
    public InputActionReference triggerAction; // Reference to your input action
    public GameObject targetObject; // The object to activate/deactivate

    void OnEnable()
    {
        triggerAction.action.Enable();
    }

    void OnDisable()
    {
        triggerAction.action.Disable();
    }

    void Update()
    {
        if (triggerAction.action.ReadValue<float>() > 0.1f)
        {
            targetObject.SetActive(true);
        }
        else
        {
            targetObject.SetActive(false);
        }
    }
}
