using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Data")]
    public string itemName;
    public Sprite itemThumbnail;
    public string itemDescription;
    public bool isFull;
    public Sprite emptySprite;
    public int itemPrice;

    [Header("Item Slot")]
    [SerializeField] private Image itemImage;
    public Image ItemImage { get { return itemImage; } set { itemImage = value; } }
    public GameObject selectedItemHighlight;
    public bool isSelected;

    [Header("Item Description")]
    public Image itemDescriptionImage;
    public TMPro.TextMeshProUGUI itemNameText;
    public TMPro.TextMeshProUGUI itemDescriptionText;
    private bool itemIsContraband;
    public TMPro.TextMeshProUGUI itemContrabandText;
    public ItemType itemType;
    public TMPro.TextMeshProUGUI itemPriceText;

    private ShopManager shopManager;


    void Start()
    {
        shopManager = FindAnyObjectByType<ShopManager>();
    }

    void Update()
    {
        
    }

    public void AddItem(string itemName, Sprite itemThumbnail, string itemDescription, bool isContraband, ItemType itemType, int itemPrice)
    {
        this.itemName = itemName;
        this.itemThumbnail = itemThumbnail;
        this.itemDescription = itemDescription;
        this.itemIsContraband = isContraband;
        this.itemType = itemType;
        this.itemPrice = itemPrice;

        isFull = true;

        itemImage.sprite = itemThumbnail;
        itemImage.color = Color.white;
        itemImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        shopManager.DeselectAllItems();
        selectedItemHighlight.SetActive(true);
        isSelected = true;

        if (itemName == "") return;

        itemDescriptionImage.sprite = itemThumbnail;
        itemNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemPriceText.text = "$" + itemPrice.ToString();

        if (itemIsContraband)
        {
            itemContrabandText.text = "Contraband";
            itemContrabandText.color = Color.red;
        }
        else
        {
            itemContrabandText.text = "Safe";
            itemContrabandText.color = Color.white;
        }

        if (itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.sprite = emptySprite;
        }

        itemDescriptionImage.color = Color.white;

    }

    public void OnRightClick()
    {
    }

    public void ClearSlot()
    {
        itemName = "";
        // itemThumbnail = null;
        // itemDescription = "";
        isFull = false;
        // itemIsContraband = false;

        itemImage.sprite = emptySprite;
        // itemImage.enabled = false;

        itemDescriptionImage.sprite = emptySprite;
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemContrabandText.text = "";

        selectedItemHighlight.SetActive(false);
        isSelected = false;
    }
}
