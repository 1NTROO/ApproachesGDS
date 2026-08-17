using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    [SerializeField] private bool playerHasContraband;
    public bool PlayerHasContraband { get { return playerHasContraband; } set { playerHasContraband = value; } }

    [Header("Enemies")]
    [SerializeField] private List<List<Transform>> enemyPatrolPointsList = new List<List<Transform>>();
    public List<List<Transform>> EnemyPatrolPointsList { get { return enemyPatrolPointsList; } set { enemyPatrolPointsList = value; } }
    // [SerializeField] private List<Transform> enemy1PatrolPoints, enemy2PatrolPoints, enemy3PatrolPoints, enemy4PatrolPoints = new List<Transform>();
    [SerializeField] private List<GameObject> enemy1PatrolPointsTemplates, enemy2PatrolPointsTemplates, enemy3PatrolPointsTemplates, enemy4PatrolPointsTemplates = new List<GameObject>();

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI levelIndexText;
    [SerializeField] private GameObject levelTransitionFade;

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
        // GetAllPatrolPoints();
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

        // if (InputSystem.actions["pause"].triggered)
        // {
        //     if (Time.timeScale == 0f)
        //     {
        //         ResumeGame();
        //     }
        //     else
        //     {
        //         PauseGame();
        //     }
            
        // }
    }

    public void LevelEnd(bool safeExit = true)
    {
        if (levelTransitionFade == null)
        {
            levelTransitionFade = GameObject.FindGameObjectWithTag("LevelTransition");
        }
        levelTransitionFade.GetComponent<LevelTransition>().FadeOutEndLevel(safeExit);        
    }

    public void LevelReset(bool safeExit = true)
    {
        PlayerStatsManager.Instance.EndOfLevelReset(safeExit);
        PlayerInventoryManager playerInventory = FindAnyObjectByType<PlayerInventoryManager>();
        playerInventory.EndOfLevel(safeExit);

        if (safeExit && PlayerStatsManager.Instance.EndGameCheck())
        {
            WinGame();
        }

        else
        {
            ChangeScene("SampleScene");
        }
    }

    public void WinGame()
    {
        Application.Quit(0);
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

    // public void GetAllPatrolPoints()
    // {
    //     if (enemyPatrolPointsList != null) return;

    //     enemyPatrolPointsList = new List<List<Transform>>
    //     {
    //         enemy1PatrolPoints,
    //         enemy2PatrolPoints,
    //         enemy3PatrolPoints,
    //         enemy4PatrolPoints
    //     };
    // }

    public List<Transform> GetPatrolPointsForEnemy(int enemyID, out List<Transform> patrolPoints)
    {
        patrolPoints = new List<Transform>();
        switch (enemyID)
        {
            case 0:
                GetRandomTemplate(enemy1PatrolPointsTemplates).GetComponentsInChildren<Transform>(true, patrolPoints);
                break;
            case 1:
                GetRandomTemplate(enemy2PatrolPointsTemplates).GetComponentsInChildren<Transform>(true, patrolPoints);
                break;
            case 2:
                GetRandomTemplate(enemy3PatrolPointsTemplates).GetComponentsInChildren<Transform>(true, patrolPoints);
                break;
            case 3:
                GetRandomTemplate(enemy4PatrolPointsTemplates).GetComponentsInChildren<Transform>(true, patrolPoints);
                break;
            default:
                Debug.LogWarning("Invalid enemyID: " + enemyID);
                break;
        }
        patrolPoints.RemoveAt(0); // Remove the parent object from the list, leaving only the child patrol points
        
        return patrolPoints;
    }

    GameObject GetRandomTemplate(List<GameObject> templates)
    {
        if (templates == null || templates.Count == 0)
        {
            Debug.LogError("Templates list is empty or null.");
            return null;
        }

        int randomIndex = Random.Range(0, templates.Count);
        return templates[randomIndex];
    }

    public void ChangeScene(string sceneName)
    {
        // ToggleShop(false);
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
