using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Data")]
    public string itemName;
    public Sprite itemThumbnail;
    public string itemDescription;
    public bool isFull;
    public Sprite emptySprite;

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


    private PlayerInventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindAnyObjectByType<PlayerInventoryManager>();
    }

    void Update()
    {
        
    }

    public void AddItem(string itemName, Sprite itemThumbnail, string itemDescription, bool isContraband, ItemType itemType)
    {
        this.itemName = itemName;
        this.itemThumbnail = itemThumbnail;
        this.itemDescription = itemDescription;
        this.itemIsContraband = isContraband;
        this.itemType = itemType;

        isFull = true;

        itemImage.sprite = itemThumbnail;
        itemImage.color = Color.white;
        itemImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Handle left-click event here
            Debug.Log("Left-clicked on item: " + itemName);
            OnLeftClick();
            // You can add additional logic here, such as using or dropping the item
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Handle right-click event here
            Debug.Log("Right-clicked on item: " + itemName);
            OnRightClick();
            // You can add additional logic here, such as using or dropping the item
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeselectAllItems();
        selectedItemHighlight.SetActive(true);
        isSelected = true;

        if (itemName == "") return;

        itemDescriptionImage.sprite = itemThumbnail;
        itemNameText.text = itemName;
        itemDescriptionText.text = itemDescription;

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
        if (inventoryManager.UseItem(itemName, true))
        {
            ClearSlot();
            selectedItemHighlight.SetActive(false);
            isSelected = false;

            print("Used item: " + itemName);
        }
    }

    private void ClearSlot()
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
    }
}
