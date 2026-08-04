using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToModify statToModify = new StatToModify();
    public int statModifyValue;
    public bool canManuallyConsume;

    public void UseItem()
    {
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

    public enum StatToModify
    {
        None,
        Stamina,
        Literacy
    }
}
