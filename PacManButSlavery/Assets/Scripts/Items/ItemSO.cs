using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToModify statToModify = new StatToModify();
    public float statModifyValue;
    public bool canManuallyConsume, canBeConsumed, isContraband;


    public void UseItem()
    {
        if (!canBeConsumed)
        {
            Debug.LogError("Item " + itemName + " cannot be consumed.");
            return;
        }
        switch (statToModify)
        {
            case StatToModify.Stamina:
                PlayerStatsManager.Instance.BulkGainStamina(statModifyValue);
                break;
            case StatToModify.Literacy:
                PlayerStatsManager.Instance.ModifyLiteracy(statModifyValue);
                break;
            default:
                Debug.LogError("Invalid stat to modify for item: " + itemName);
                break;
        }
    }

    public void EquipItem()
    {
        switch (statToModify)
        {
            case StatToModify.Movement:
                PlayerStatsManager.Instance.ModifySpeed(statModifyValue);
                break;
            case StatToModify.Stamina:
                PlayerStatsManager.Instance.ModifyStaminaConsumption(statModifyValue);
                break;
            case StatToModify.Threat:
                PlayerStatsManager.Instance.ModifyThreatDetection(statModifyValue);
                break;
            case StatToModify.Money:
                PlayerStatsManager.Instance.ModifyMoneyGain(statModifyValue);
                break;
            default:
                Debug.LogError("Invalid stat to modify for item: " + itemName);
                break;
        }
    }

    public enum StatToModify
    {
        None,
        Stamina,
        Literacy,
        Threat,
        Movement,
        Money,
        Bonus
    }
}
