using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Settings")]
    public List<GameObject> items = new List<GameObject>(); // Array to hold all items available in the shop
    public GameObject foodItem;
    public ItemSO[] itemSOs; // Array to hold scriptable objects for each item
    private List<GameObject> itemsInShop = new List<GameObject>(); // Array to hold the items currently in the shop

    [Header("UI Settings")]
    public GameObject shopPanel; // Reference to the shop UI panel
    public ShopItemSlot[] itemSlots; // Array to hold the item slots in the shop UI
    public Button buyButton; // Reference to the buy button in the shop UI
    public Image shopInteractionPrompt; // Reference to the shop interaction prompt UI element

    private bool isFaded = true; // Flag to check if the shop interaction prompt is faded in or out

    private PlayerMovement playerMovement; // Reference to the PlayerMovement script

    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>(); // Get the PlayerMovement script from the player object
        shopInteractionPrompt.DOFade(0f, 0f); // Ensure the shop interaction prompt is hidden at the start
        shopInteractionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(0f, 0f); // Ensure the prompt text is hidden at the start
        GenerateShopItems(); // Generate the items in the shop at the start
        if (shopPanel != null)
        {
            shopPanel.SetActive(false); // Ensure the shop panel is hidden at the start
        }
        else
        {
            Debug.LogError("Shop Panel is not assigned in the inspector.");
        }
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(() =>
            {
                foreach (var slot in itemSlots)
                {
                    if (slot.isSelected)
                    {
                        if (BuyItem(slot.itemName))
                        {
                            slot.ClearSlot();
                        }
                        else
                        {
                            Debug.Log("Failed to buy item: " + slot.itemName);
                        }
                        break;
                    }
                }
            });
        }
        else
        {
            Debug.LogError("Buy Button is not assigned in the inspector.");
        }
    }

    void Update()
    {
        if (PlayerDistanceCheck())
        {
            if (InputSystem.actions["Interact"].triggered)
            {
                if (shopPanel.activeSelf)
                {
                    CloseShop();
                }
                else
                {
                    OpenShop();
                }
            }
        }
    }

    public void OpenShop()
    {
        // Logic to open the shop UI and display items
        // You can instantiate item prefabs or use UI elements to show the items
        shopPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game when the shop is open
    }

    public void CloseShop()
    {
        // Logic to close the shop UI
        shopPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game when the shop is closed
    }

    public bool PlayerDistanceCheck()
    {
        if (Vector2.Distance(playerMovement.transform.position, transform.position) <= 1.5f) // Check if the player is within interaction range
        {
            if (isFaded)
            {
                shopInteractionPrompt.DOFade(1f, 0.3f); // Fade in the shop interaction prompt
                shopInteractionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(1f, 0.3f); // Fade in the prompt text
                isFaded = false;
            }
            return true; // Player is close enough to interact with the shop
        }
        else
        {
            if (!isFaded)
            {
                shopInteractionPrompt.DOFade(0f, 0.3f); // Fade out the shop interaction prompt
                shopInteractionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(0f, 0.3f); // Fade out the prompt text
                isFaded = true;
            }
            return false; // Player is too far away to interact with the shop
        }
    }

    void GenerateShopItems()
    {
        itemsInShop.Clear(); // Clear the current items in the shop
        foreach (var slot in itemSlots)
        {
            slot.ClearSlot(); // Clear each item slot in the shop UI
        }

        List<GameObject> tempItems = new List<GameObject>(items); // Create a temporary list of items to choose from
        List<int> randomNumbers = new List<int>(); // List to hold random numbers for item selection
        for (int i = 0; i < 5; i++) // Generate 5 random items for the shop
        {
            int randomIndex = Random.Range(0, tempItems.Count); // Get a random index from the temporary list
            if (randomNumbers.Contains(randomIndex)) // Check if the random index has already been used
            {
                i--; // If it has, decrement i to try again
                continue; // Skip to the next iteration
            }
            else
            {
                randomNumbers.Add(randomIndex); // If it hasn't, add it to the list of used indices
            }
            GameObject selectedItem = tempItems[randomIndex]; // Select the item at the random index
            itemsInShop.Add(selectedItem); // Add the selected item to the shop
        }
        itemsInShop.Add(foodItem); // Add the food item to the shop

        for (int j = 0; j < itemsInShop.Count; j++) // Loop through the items in the shop
        {
            GameObject item = itemsInShop[j];
            ItemBase itemBase = item.GetComponent<ItemBase>(); // Get the ItemBase component of the item
            if (itemBase != null) // Check if the ItemBase component exists
            {
                itemSlots[j].AddItem(itemBase.name, itemBase.itemThumbnail, itemBase.ItemDescription, itemBase.IsContraband, itemBase.thisItemType, itemBase.price); // Add the item to the slot
            }
            else
            {
                Debug.LogError("ItemBase component not found on " + item.name); // Log an error if the ItemBase component is missing
            }
        }
    }

    public bool BuyItem(string itemName)
    {
        // Logic to handle buying an item
        // You can check if the player has enough currency, deduct the cost, and add the item to the player's inventory
        foreach (var i in items)
        {
            if (i.name == itemName)
            {
                var values = i.GetComponent<ItemBase>();
                // Check if the player has enough currency
                if (PlayerStatsManager.Instance.SpendMoney(values.price))
                {
                    // Add the item to the player's inventory
                    PlayerInventoryManager inventoryManager = FindAnyObjectByType<PlayerInventoryManager>();
                    if (inventoryManager != null)
                    {
                        inventoryManager.AddItemToInventory(values.name, values.itemThumbnail, values.ItemDescription, values.IsContraband, values.thisItemType);
                    }
                    return true; // Item purchased successfully

                }
                else
                {
                    Debug.Log("Not enough money to buy " + itemName);
                    return false; // Not enough money
                }
            }
        }
        return false; // Item not found
    }

    public void DeselectAllItems()
    {
        foreach (var slot in itemSlots)
        {
            slot.selectedItemHighlight.SetActive(false);
            slot.isSelected = false;
        }
    }
}
