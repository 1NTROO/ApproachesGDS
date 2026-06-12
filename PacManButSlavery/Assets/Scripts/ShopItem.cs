using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    private enum ItemType {
        Stamina,
        Free
    }
    [Header("Shop Settings")]
    [SerializeField] private ItemType thisItem;
    [SerializeField] private int priceBase;
    [SerializeField] private float priceMult; 
    private int currentPrice;

    private Button buyButton;
    private TMPro.TextMeshProUGUI shopText;
    private Image itemImage;
    
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void OnShopLoad(int itemTypeInt)
    {
        thisItem = (ItemType)itemTypeInt;

        buyButton = GetComponentInChildren<Button>();
        itemImage = GetComponentInChildren<Image>();

        buyButton.onClick.AddListener(BuyItem);

        switch (thisItem)
        {
            case ItemType.Stamina:
                priceBase = 75;
                priceMult = 2;
                currentPrice = priceBase;
                buyButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Buy\n" + currentPrice;
                break;
            case ItemType.Free:
                priceBase = 5000;
                priceMult = 1;
                currentPrice = priceBase;
                buyButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Buy\n" + currentPrice;
                break;
            default:
                break;
        }
    }

    void BuyItem()
    {
        if (PlayerStatsManager.Instance.SpendMoney(currentPrice))
        {
            currentPrice *= (int)priceMult;

            switch (thisItem)
            {
                case ItemType.Stamina:
                    PlayerStatsManager.Instance.BulkGainStamina(10);
                    break;
                case ItemType.Free:
                    // end the game
                    break;
                default:
                    break;
            }

            buyButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Buy\n" + currentPrice;
        }
    }
}
