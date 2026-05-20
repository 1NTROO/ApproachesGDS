using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    PlayerStatsManager statsManager;

    [Header("Stamina UI")]
    [SerializeField] private Slider staminaBar;

    void Start()
    {
        statsManager = PlayerStatsManager.Instance;

        if (staminaBar == null)
        {
            staminaBar = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<Slider>();
            if (staminaBar == null)
            {
                Debug.LogError("StaminaBar Slider not found in the scene. Please assign it in the inspector or tag it as 'StaminaBar'.");
            }
        }

        staminaBar.value = statsManager.CurrentStamina;
    }

    void Update()
    {
        UpdateStaminaUI();
    }

    public void ConsumeStamina(float amount)
    {
        statsManager.CurrentStamina -= amount;
        statsManager.CurrentStamina = Mathf.Clamp(statsManager.CurrentStamina, 0, statsManager.MaxStamina);
    }

    public void RegenerateStamina(float amount)
    {
        statsManager.CurrentStamina += amount;
        statsManager.CurrentStamina = Mathf.Clamp(statsManager.CurrentStamina, 0, statsManager.MaxStamina);
    }
    public void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = statsManager.CurrentStamina;
        }
    }
}
