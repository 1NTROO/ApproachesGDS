using System.Collections.Generic;
using System.Numerics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject pickupParent;

    private float ingameTime = 7f;

    [Header("Pickups")]
    private int currentPickupCount;
    [SerializeField] private int pickupCountAtStart = 350;
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private List<GameObject> templatesLeft = new List<GameObject>();
    [SerializeField] private List<GameObject> templatesRight = new List<GameObject>();

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI ingameTimeText;

    void Start()
    {
        GameManager.Instance.ShopCanvas = GameObject.FindGameObjectWithTag("ShopCanvas");
        GameManager.Instance.ToggleShop(false);

        GeneratePickups(GetRandomTemplate(templatesLeft));
        GeneratePickups(GetRandomTemplate(templatesRight));
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

    void GeneratePickups(GameObject obj)
    {
        Transform[] coordinatesList = obj.GetComponentsInChildren<Transform>(true);

        foreach (Transform coord in coordinatesList)
        {
            Instantiate(pickupPrefab, coord.position, coord.rotation, pickupParent.transform);
        }
    }

    GameObject GetRandomTemplate(List<GameObject> list)
    {
        GameObject obj = null;
        obj = list[Random.Range(0, list.Count)];
        if (obj == null)
        {
            Debug.LogError("Random template object not found.");
        }
        return obj;
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
        int startPickup25 = Mathf.FloorToInt(pickupCountAtStart / 25);
        int currentPickup25 = Mathf.FloorToInt(pickupCount / 25);
        ingameTime = 7f + (startPickup25 - currentPickup25);

        if (ingameTimeText != null)
        {
            string displayText = "";
            if (ingameTime < 10f) displayText += "0";
            displayText += ingameTime.ToString() + ":00";
            ingameTimeText.text = displayText;
        }
    }
}
