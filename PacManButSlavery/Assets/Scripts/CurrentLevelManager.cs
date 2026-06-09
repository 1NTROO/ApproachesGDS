using UnityEngine;

public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject pickupParent;

    private float ingameTime = 7f;

    private int currentPickupCount;
    private int pickupCountAtStart = 134;

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI ingameTimeText;

    void Start()
    {
        GameManager.Instance.ShopCanvas = GameObject.FindGameObjectWithTag("ShopCanvas");
        GameManager.Instance.ToggleShop(false);
    }

    void Update()
    {
        if (ingameTimeText == null)
        {
            ingameTimeText = GameObject.FindGameObjectWithTag("IngameTimeText").GetComponent<TMPro.TextMeshProUGUI>();
            if (ingameTimeText == null)
            {
                Debug.LogError("IngameTimeText TextMeshProUGUI not found in the scene. Please assign it in the inspector or tag it as 'IngameTimeText'.");
            }
        }
    }

    public void CheckEndLevel()
    {
        if (GetPickupCount() == 0)
        {
            Debug.Log("Level Complete!");
            PlayerStatsManager.Instance.EndOfLevelReset();
        }
        else
        {
            currentPickupCount = GetPickupCount();
            ChangeInGameTime(currentPickupCount);
        }
    }

    int GetPickupCount()
    {
        int pickupCount = 0;
        foreach (Transform child in pickupParent.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                continue;
            }
            pickupCount++;
        }
        return pickupCount;
    }

    void ChangeInGameTime(int pickupCount)
    {
        int startPickup10 = Mathf.FloorToInt(pickupCountAtStart / 10);
        int currentPickup10 = Mathf.FloorToInt(pickupCount / 10);
        ingameTime = 7f + (startPickup10 - currentPickup10);

        if (ingameTimeText != null)
        {
            string displayText = "";
            if (ingameTime < 10f) displayText += "0";
            displayText += ingameTime.ToString() + ":00";
            ingameTimeText.text = displayText;
        }
    }
}
