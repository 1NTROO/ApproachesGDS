using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }


    [Header("Shop")]
    [SerializeField] private GameObject shopCanvas;
    public GameObject ShopCanvas { get { return shopCanvas; } set { shopCanvas = value; } }
    [SerializeField] private ShopItem shopItem1;
    public ShopItem ShopItem1 { get { return shopItem1; } set { shopItem1 = value; } }
    [SerializeField] private ShopItem shopItem2;
    public ShopItem ShopItem2 { get { return shopItem2; } set { shopItem2 = value; } }

    private int shopPrice1, shopPrice2;

    [Header("Player")]
    [SerializeField] private Transform transformPlayer;
    public Transform TransformPlayer { get { return transformPlayer; } set { transformPlayer = value; } }

    [Header("Enemies")]
    [SerializeField] private Transform[,] enemyPatrolPoints;
    public Transform[,] EnemyPatrolPoints { get { return enemyPatrolPoints; } set { enemyPatrolPoints = value; } }
    [SerializeField] private Transform[] enemy1PatrolPoints, enemy2PatrolPoints, enemy3PatrolPoints, enemy4PatrolPoints;

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI levelIndexText;

    private int levelIndex = 0;


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

    void Start()
    {
        GetAllPatrolPoints();
    }

    void Update()
    {
        if (levelIndexText == null)
        {
            levelIndexText = GameObject.FindGameObjectWithTag("LevelIndexText").GetComponent<TMPro.TextMeshProUGUI>();
            if (levelIndexText == null)
            {
                Debug.LogError("LevelIndexText TextMeshProUGUI not found in the scene. Please assign it in the inspector or tag it as 'LevelIndexText'.");
            }
        }
        else
        {
            if (levelIndexText.text != "Day: " + (levelIndex + 1).ToString())
            {
                levelIndexText.text = "Day: " + (levelIndex + 1).ToString();
            }
        }

        if (InputSystem.actions["pause"].triggered)
        {
            if (Time.timeScale == 0f)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
            
        }
    }

    public void ToggleShop(bool isActive)
    {
        if (shopCanvas != null)
        {
            shopCanvas.SetActive(isActive);
            
            Time.timeScale = isActive ? 0f : 1f;

            if (!isActive) return;

            shopItem1 = shopCanvas.GetComponentsInChildren<ShopItem>()[0];
            shopItem2 = shopCanvas.GetComponentsInChildren<ShopItem>()[1];

            shopItem1.OnShopLoad(0);
            shopItem2.OnShopLoad(1);

        }
    }

    public void GetAllPatrolPoints()
    {
        if (enemyPatrolPoints != null) return;

        enemyPatrolPoints = new Transform[4, 4]
        {
            { enemy1PatrolPoints[0], enemy1PatrolPoints[1], enemy1PatrolPoints[2], enemy1PatrolPoints[3] },
            { enemy2PatrolPoints[0], enemy2PatrolPoints[1], enemy2PatrolPoints[2], enemy2PatrolPoints[3] },
            { enemy3PatrolPoints[0], enemy3PatrolPoints[1], enemy3PatrolPoints[2], enemy3PatrolPoints[3] },
            { enemy4PatrolPoints[0], enemy4PatrolPoints[1], enemy4PatrolPoints[2], enemy4PatrolPoints[3] }
        };
    }

    public Transform[] GetPatrolPointsForEnemy(int enemyID, out Transform[] patrolPoints)
    {
        patrolPoints = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            patrolPoints[i] = enemyPatrolPoints[enemyID, i];
        }
        return patrolPoints;
    }

    public void ChangeScene(string sceneName)
    {
        ToggleShop(false);
        SceneManager.LoadScene(sceneName);
        Debug.Log("Changing scene to: " + sceneName);
        levelIndex++;

    }

    void PauseGame()
    {
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
