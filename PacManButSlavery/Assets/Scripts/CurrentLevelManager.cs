using System.Collections.Generic;
using System.Numerics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject pickupParent;
    [SerializeField] private GameObject specialPickupParent;

    private float ingameTime = 7f;

    [Header("Regular Pickups")]
    private int currentPickupCount;
    [SerializeField] private int pickupCountAtStart = 350;
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private List<GameObject> templatesLeft = new List<GameObject>();
    [SerializeField] private List<GameObject> templatesRight = new List<GameObject>();

    [Header("Special Pickups")]
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private GameObject spawnLocationObj;
    private List<Transform> spawnLocations = new List<Transform>();
    [SerializeField] private float spawnTimer = 20.0f;
    private float spawnTimerCurrent = 0.0f;
    private float specialSpawnOdds = 0.0f;


    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI ingameTimeText;

    void Start()
    {
        GameManager.Instance.ShopCanvas = GameObject.FindGameObjectWithTag("ShopCanvas");
        GameManager.Instance.ToggleShop(false);

        GeneratePickups(GetRandomTemplate(templatesLeft));
        GeneratePickups(GetRandomTemplate(templatesRight));

        spawnLocationObj.GetComponentsInChildren<Transform>(true, spawnLocations);
    }

    void Update()
    {
        spawnTimerCurrent += Time.deltaTime;
        if (spawnTimerCurrent >= spawnTimer) SpecialPickupLogic();

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
        List<Transform> coordinatesList = new List<Transform>();
        obj.GetComponentsInChildren<Transform>(true, coordinatesList);

        coordinatesList.RemoveAt(0); // Remove the first item, which is the Transform of the parent object. 

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

    void SpecialPickupLogic()
    {
        float rand = Random.value;
        if (rand <= 1 / (1 + specialSpawnOdds))
        {
            if (spawnLocations.Count <= 1)
            {
                Debug.LogError("No spawn locations found for special pickups.");
                return;
            }
            int randomIndex = Random.Range(1, spawnLocations.Count); // Starts at 1 because at index 0 the Transform of the parent object lives (which is [0, 0, 0]).
            Transform t = spawnLocations[randomIndex];
            Instantiate(moneyPrefab, t.position, t.rotation);
            spawnLocations.RemoveAt(randomIndex);
        }
        specialSpawnOdds++;
        spawnTimerCurrent = 0;
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
