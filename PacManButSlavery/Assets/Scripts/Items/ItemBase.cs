using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ItemBase : MonoBehaviour
{
    [Header("Item Type Settings")]

    public ItemType thisItemType;
    [SerializeField] bool isContraband = false;
    public bool IsContraband    { 
                                get { return isContraband; } 
                                set { isContraband = value; } 
                                }
    

    [Header("Item Shop Settings")]
    public int price;
    public Sprite itemThumbnail;
    [TextArea] [SerializeField] private string itemDescription;
    public string ItemDescription { 
                                    get { return itemDescription; } 
                                    set { itemDescription = value; } 
                                    }
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddToInventory()
    {
        PlayerInventoryManager inventoryManager = FindAnyObjectByType<PlayerInventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.AddItemToInventory(gameObject.name, itemThumbnail, itemDescription, isContraband, thisItemType);
            Debug.Log("Added " + gameObject.name + " to inventory.");
        }
        else
        {
            Debug.LogError("PlayerInventoryManager not found in the scene.");
        }
    }
}
