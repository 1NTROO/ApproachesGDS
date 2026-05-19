using UnityEngine;

public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject pickupParent;
    void Start()
    {
        GameManager.Instance.ShopCanvas = GameObject.FindGameObjectWithTag("ShopCanvas");
        GameManager.Instance.ToggleShop(false);
    }

    void Update()
    {
        
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
            Debug.Log("Pickups remaining: " + GetPickupCount());
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
}
