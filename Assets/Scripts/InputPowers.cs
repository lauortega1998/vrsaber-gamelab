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

        if (playerStats != null && rageSystem != null && rageSystem.IsRageActive())
        {
            // Fire Power
            if (leftTriggerValue > 0.1f && playerStats.CurrentMana > 0)
            {
                firePowerObject.SetActive(true);
                firePowerTutorial.SetActive(false);
                FindAnyObjectByType<AudioManager>().Play("flamethrower");
            }
            else
            {
                firePowerObject.SetActive(false);
                FindAnyObjectByType<AudioManager>().Stop("flamethrower");

            }

            // Ice Power
            if (rightTriggerValue > 0.1f && playerStats.CurrentMana > 0)
            {
                icePowerObject.SetActive(true);
                icePowerTutorial.SetActive(false);
                FindAnyObjectByType<AudioManager>().Play("icethrower");

            }
            else
            {
                icePowerObject.SetActive(false);
                FindAnyObjectByType<AudioManager>().Stop("icethrower");

            }
        }
        else
        {
            firePowerObject.SetActive(false);
            icePowerObject.SetActive(false);
        }

        // Still allow the rage input logic to happen
        if (rageSystem != null)
        {
            if (leftTriggerValue > 0.1f || rightTriggerValue > 0.1f)
            {
                rageSystem.TryActivateRageFromInput();
            }
        }
    }
}

