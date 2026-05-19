using UnityEngine;
using UnityEngine.SceneManagement;

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
        
    }

    void Update()
    {
        
    }

    public void ToggleShop(bool isActive)
    {
        if (shopCanvas != null)
        {
            shopCanvas.SetActive(isActive);
            Time.timeScale = isActive ? 0f : 1f;
        }
    }

    public void ChangeScene(string sceneName)
    {
        ToggleShop(false);
        SceneManager.LoadScene(sceneName);
        Debug.Log("Changing scene to: " + sceneName);
    }
}
