using UnityEngine;
using UnityEngine.InputSystem;

public class InputPowers : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference leftTriggerAction;
    public InputActionReference rightTriggerAction;

    [Header("Power Objects")]
    public GameObject firePowerObject;
    public GameObject icePowerObject;

    public GameObject firePowerTutorial;
    public GameObject icePowerTutorial;

    public PlayerStats playerStats;

    [Header("Rage System Reference")]
    public RageSystem rageSystem; // 🔁 NEW

    void OnEnable()
    {
        leftTriggerAction.action.Enable();
        rightTriggerAction.action.Enable();
        playerStats = GetComponent<PlayerStats>();
    }

    void OnDisable()
    {
        leftTriggerAction.action.Disable();
        rightTriggerAction.action.Disable();
    }

    void Update()
    {
        float leftTriggerValue = leftTriggerAction.action.ReadValue<float>();
        float rightTriggerValue = rightTriggerAction.action.ReadValue<float>();

        if (playerStats != null)
        {
            // Fire Power
            if (leftTriggerValue > 0.1f && playerStats.CurrentMana > 0)
            {
                firePowerObject.SetActive(true);
                firePowerTutorial.SetActive(false);

                Debug.Log("Fire Trigger Pressed – Rage Attempt");
                rageSystem?.TryActivateRageFromInput();
            }
            else
            {
                firePowerObject.SetActive(false);
            }

            // Ice Power
            if (rightTriggerValue > 0.1f && playerStats.CurrentMana > 0)
            {
                icePowerObject.SetActive(true);
                icePowerTutorial.SetActive(false);
                Debug.Log("Ice Trigger Pressed – Rage Attempt");

                rageSystem?.TryActivateRageFromInput(); // 🔁 Try trigger Rage
            }
            else
            {
                icePowerObject.SetActive(false);
            }
        }
    }
}

