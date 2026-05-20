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
    [SerializeField] private ShopItem shopItem1;
    public ShopItem ShopItem1 { get { return shopItem1; } set { shopItem1 = value; } }
    [SerializeField] private ShopItem shopItem2;
    public ShopItem ShopItem2 { get { return shopItem2; } set { shopItem2 = value; } }

    private int shopPrice1, shopPrice2;

    [Header("Player")]
    [SerializeField] private Vector3 transformPlayer;
    public Vector3 TransformPlayer { get { return transformPlayer; } set { transformPlayer = value; } }


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

            if (!isActive) return;

            shopItem1 = shopCanvas.GetComponentsInChildren<ShopItem>()[0];
            shopItem2 = shopCanvas.GetComponentsInChildren<ShopItem>()[1];

            shopItem1.OnShopLoad(0);
            shopItem2.OnShopLoad(1);

        }
    }

    public void ChangeScene(string sceneName)
    {
        ToggleShop(false);
        SceneManager.LoadScene(sceneName);
        Debug.Log("Changing scene to: " + sceneName);
    }
}
