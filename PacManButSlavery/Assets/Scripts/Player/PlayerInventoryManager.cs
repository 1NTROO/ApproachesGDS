using Unity.Mathematics;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public ItemSlot[] itemSlots;
    public ItemSO[] itemSOs;
    private bool isCarryingContraband = false;
    public bool IsCarryingContraband { get { return isCarryingContraband; } }
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
                    return false; // Cannot use the item if the manualConsume flag does not match the item's canManuallyConsume property
                }
                SO.UseItem();
                if (!ContrabandCheck(itemName))
                {
                    isCarryingContraband = false;
                }
                return true;
            }
        }
        return false;
    }

    public bool EquipItem(string itemName)
    {
        foreach (var SO in itemSOs)
        {
            if (SO.itemName == itemName)
            {
                if (SO.canBeConsumed)
                {
                    return false; // Cannot equip an item that can be consumed
                }
                SO.EquipItem();
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
                EquipItem(itemName); // Equip the item when added to inventory
                if (ContrabandCheck(itemName))
                {
                    isCarryingContraband = true;
                }
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

    private bool ContrabandCheck(string itemName)
    {
        foreach (var SO in itemSOs)
        {
            if (SO.itemName == itemName && SO.isContraband)
            {
                GameManager.Instance.PlayerHasContraband = true;
                return true;
            }
        }
        GameManager.Instance.PlayerHasContraband = false;
        return false;
    }
}

public enum ItemType
{
    None,
    Stamina,
    Movement,
    Threat,
    Literacy,
    Money,
    Bonus
}
