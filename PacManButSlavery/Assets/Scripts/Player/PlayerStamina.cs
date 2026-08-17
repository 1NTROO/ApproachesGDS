using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    PlayerStatsManager statsManager;

    [Header("Stamina UI")]
    [SerializeField] private Slider staminaBar;

    [Header("Audio")]
    [SerializeField] private AudioClip staminaRegenerationSound;


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

        DeathCheck();
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

        AudioManager.Instance.PlaySound(staminaRegenerationSound, 0.1f);
    }
    public void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = statsManager.CurrentStamina;
        }
    }

    public void DeathCheck()
    {
        if (statsManager.CurrentStamina > 0 || statsManager.Nutrition > 0) return;
        else
        {
            Destroy(PlayerInventoryManager.Instance.gameObject);
            Destroy(PlayerStatsManager.Instance.gameObject);
            Destroy(AudioManager.Instance.gameObject);
            Destroy(GameManager.Instance.gameObject);
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
