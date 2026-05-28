using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatsManager : MonoBehaviour
{
    private static PlayerStatsManager instance;
    public static PlayerStatsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<PlayerStatsManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PlayerStatsManager");
                    instance = obj.AddComponent<PlayerStatsManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("Points")]
    [SerializeField] private int pointsPerPickup = 10;
    public int PointsPerPickup { get { return pointsPerPickup; } }
    [SerializeField] private int pointsTotal = 0;

    [Space(10)]
    [SerializeField] private TMPro.TextMeshProUGUI pointsUI;

    [Header("Money")]
    [SerializeField] private int moneyPerPickup = 50;
    public int MoneyPerPickup { get { return moneyPerPickup; } }
    [SerializeField] private int moneyTotal = 0;

    [Space(10)]
    [SerializeField] private TMPro.TextMeshProUGUI moneyUI;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    public float MaxStamina { get { return maxStamina; } }
    [SerializeField] private float currentStamina = 100f;
    public float CurrentStamina { get { return currentStamina; } set { currentStamina = value; } }

    private bool staminaStarted = false;

    void Start()
    {
        if (staminaStarted)
        {
            return;
        }
        else
        {
            staminaStarted = true;
            currentStamina = maxStamina;
        }


    }

    void Update()
    {
        if (pointsUI == null)
        {
            pointsUI = GameObject.FindGameObjectWithTag("PointsUI").GetComponent<TMPro.TextMeshProUGUI>();
            UpdatePointsUI();
            if (pointsUI == null)
            {
                Debug.LogError("PointsUI TextMeshProUGUI not found in the scene. Please assign it in the inspector or tag it as 'PointsUI'.");
            }
        }

        if (moneyUI == null)
        {
            moneyUI = GameObject.FindGameObjectWithTag("MoneyUI").GetComponent<TMPro.TextMeshProUGUI>();
            UpdateMoneyUI();
            if (moneyUI == null)
            {
                Debug.LogError("MoneyUI TextMeshProUGUI not found in the scene. Please assign it in the inspector or tag it as 'MoneyUI'.");
            }
        }  
    }

    public void AddPoints(int amount)
    {
        pointsTotal += amount;
        Debug.Log("Points: " + pointsTotal);

        UpdatePointsUI();
    }

    public void AddMoney(int amount)
    {
        moneyTotal += amount;
        Debug.Log("Money: " + moneyTotal);

        UpdateMoneyUI();
    }

    public void UpdatePointsUI()
    {
        if (pointsUI != null)
        {
            pointsUI.text = "Points: " + pointsTotal;
        }
    }

    public void UpdateMoneyUI()
    {
        if (moneyUI != null)
        {
            moneyUI.text = "Money: " + moneyTotal;
        }
    }

    public void BulkGainStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    public bool SpendMoney(int amount)
    {
        if (moneyTotal < amount)
            return false;
        else
        {
            moneyTotal -= amount;
            UpdateMoneyUI();
            return true;
        }
    }

    public void EndOfLevelReset()
    {
        pointsTotal = 0;

        pointsUI = null;
        moneyUI = null;

        BulkGainStamina((maxStamina - currentStamina) * 0.35f + // Gain 35% of missing stamina always
                        (currentStamina / maxStamina * 25f));   // Gain up to 25% of max stamina based on current stamina percentage, gaining more stamina if the player has more stamina left
                                                                // Rewards higher stamina, simulating exhaustion and recovery, while making it possible for players to die if they are not careful.

        GameManager.Instance.ToggleShop(true);
    }

    public void TakeStaminaDamage(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (currentStamina <= 0)
        {
            Debug.Log("Player has run out of stamina! Game Over.");
            // Handle game over logic here (e.g., reload scene, show game over screen, etc.)
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("Player took stamina damage! Current Stamina: " + currentStamina);
            // StartCoroutine(TakeStaminaDamageCoroutine(amount));
        }
    }
    
}
