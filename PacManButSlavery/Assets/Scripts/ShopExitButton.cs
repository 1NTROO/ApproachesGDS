using UnityEngine;
using UnityEngine.UI;

public class ShopExitButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "SampleScene";
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnExitButtonClicked);
        }
        else
        {
            Debug.LogError("ShopExitButton: No Button component found on this GameObject.");
        }
    }

    void Update()
    {
        
    }

    void OnExitButtonClicked()
    {
        GameManager.Instance.ChangeScene(sceneToLoad);
    }
}
