using Unity.Mathematics;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    private static PlayerInventoryManager instance;
    public static PlayerInventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<PlayerInventoryManager>();
                if (instance == null)
                {
                    return null;
                }
            }
            return instance;
        }
    }

    public GameObject inventoryPanel;
    public ItemSlot[] itemSlots;
    public ItemSO[] itemSOs;
    private bool isCarryingContraband = false;
    public bool IsCarryingContraband { get { return isCarryingContraband; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
                isCarryingContraband = ItemSlotsContrabandCheck();
                GameManager.Instance.PlayerHasContraband = isCarryingContraband;
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
                isCarryingContraband = ContrabandCheck(itemName);
                GameManager.Instance.PlayerHasContraband = isCarryingContraband;
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
                return true;
            }
        }
        return false;
    }

    public bool ItemSlotsContrabandCheck()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (ContrabandCheck(itemSlots[i].name)) return true;
        }
        return false;
    }

    public void EndOfLevel(bool safeExit = true)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            foreach (var SO in itemSOs)
            {
                if (SO.itemName != itemSlots[i].itemName) continue;
                else if (!safeExit)
                {
                    if (SO.isContraband)
                    {
                        itemSlots[i].ClearSlot();
                        break;
                    }
                }
                else if (SO.canBeConsumed && !SO.canManuallyConsume)
                {
                    UseItem(SO.itemName, false);
                    itemSlots[i].ClearSlot();
                    break;
                }
            }
        }
        isCarryingContraband = ItemSlotsContrabandCheck();
        GameManager.Instance.PlayerHasContraband = isCarryingContraband;
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
