using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxMana = 100;
    [SerializeField] private int currentMana;
    [SerializeField] private float manaDrainRate = 10f;
    public EnemyHealth enemyhealth;

    [Header("Input")]
    [SerializeField] private InputActionReference useManaAction;
    [SerializeField] private InputActionReference useManaActionFire;


    [Header("UI")]
    [SerializeField] private TextMeshProUGUI manaText; // ← Drag the Text UI here

    private bool isDraining = false;
    
    public int CurrentMana => currentMana; // Getter for currentMana

    private void Start()
    {
        currentMana = maxMana;
        UpdateManaUI();

        // Enable and listen for the start and cancel actions for draining mana
        useManaAction.action.started += ctx => isDraining = true;
        useManaAction.action.canceled += ctx => isDraining = false;

        useManaAction.action.Enable();

        useManaActionFire.action.started += ctx => isDraining = true;
        useManaActionFire.action.canceled += ctx => isDraining = false;

        useManaActionFire.action.Enable();
    }

    private void OnDestroy()
    {
        useManaAction.action.started -= ctx => isDraining = true;
        useManaAction.action.canceled -= ctx => isDraining = false;

        useManaActionFire.action.started -= ctx => isDraining = true;
        useManaActionFire.action.canceled -= ctx => isDraining = false;
    }

    private void Update()
    {
        // Drain mana if the player is using it
        if (isDraining && currentMana > 0)
        {
            float manaToDrain = manaDrainRate * Time.deltaTime;
            currentMana -= Mathf.CeilToInt(manaToDrain);
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);

            UpdateManaUI();

            if (currentMana <= 0)
            {
                Die();
                isDraining = false;
            }
        }
    }

    public void GainMana(int amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        UpdateManaUI();
    }

    private void UpdateManaUI()
    {
        if (manaText != null)
        {
            manaText.text = $"Mana: {currentMana}/{maxMana}";
        }
    }

    private void Die() // Apply what happens when the mana is 0
    {
        Debug.Log("Player has no mana left and died.");
    }
}