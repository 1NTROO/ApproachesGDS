using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public ItemSlot[] itemSlots;
    public ItemSO[] itemSOs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool UseItem(string itemName, bool manualConsume)
    {
        foreach (var SO in itemSOs)
        {
            if (SO.itemName == itemName)
            {
                if (manualConsume != SO.canManuallyConsume)
                {
                    return false;
                }
                SO.UseItem();
                return true;
            }
        }
        return false;
    }

    public void AddItemToInventory(string itemName, Sprite itemThumbnail, string itemDescription, bool isContraband, ItemType itemType)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFull)
            {
                itemSlots[i].AddItem(itemName, itemThumbnail, itemDescription, isContraband, itemType);
                break;
            }
        }
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

public enum ItemType
{
    Stamina,
    Movement,
    Threat,
    Literacy,
    Bonus
}
