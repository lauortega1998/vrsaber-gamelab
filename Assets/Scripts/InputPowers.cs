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
    public RageSystem rageSystem;

    private bool isFirePlaying = false;
    private bool isIcePlaying = false;

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
            // FIRE POWER
            if (leftTriggerValue > 0.1f && playerStats.CurrentMana > 0)
            {
                firePowerObject.SetActive(true);
                firePowerTutorial.SetActive(false);

                if (!isFirePlaying)
                {
                    FindAnyObjectByType<AudioManager>().Play("flamethrower");
                    isFirePlaying = true;
                }
            }
            else
            {
                firePowerObject.SetActive(false);
                if (isFirePlaying)
                {
                    FindAnyObjectByType<AudioManager>().Stop("flamethrower");
                    isFirePlaying = false;
                }
            }

            // ICE POWER
            if (rightTriggerValue > 0.1f && playerStats.CurrentMana > 0)
            {
                icePowerObject.SetActive(true);
                icePowerTutorial.SetActive(false);

                if (!isIcePlaying)
                {
                    FindAnyObjectByType<AudioManager>().Play("icethrower");
                    isIcePlaying = true;
                }
            }
            else
            {
                icePowerObject.SetActive(false);
                if (isIcePlaying)
                {
                    FindAnyObjectByType<AudioManager>().Stop("icethrower");
                    isIcePlaying = false;
                }
            }
        }
        else
        {
            firePowerObject.SetActive(false);
            icePowerObject.SetActive(false);

            if (isFirePlaying)
            {
                FindAnyObjectByType<AudioManager>().Stop("flamethrower");
                isFirePlaying = false;
            }

            if (isIcePlaying)
            {
                FindAnyObjectByType<AudioManager>().Stop("icethrower");
                isIcePlaying = false;
            }
        }

        // Rage input logic
        if (rageSystem != null && (leftTriggerValue > 0.1f || rightTriggerValue > 0.1f))
        {
            rageSystem.TryActivateRageFromInput();
        }
    }
}
