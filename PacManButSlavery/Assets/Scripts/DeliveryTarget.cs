using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class DeliveryTarget : MonoBehaviour
{
    [SerializeField] private Image deliveryInteractionPrompt;
    private bool isFaded = true;
    private CurrentLevelManager currentLevelManager;
    private PlayerMovement playerMovement;
    private int lastPickupCount = 350;
    void Start()
    {
        currentLevelManager = FindAnyObjectByType<CurrentLevelManager>();
        playerMovement = FindAnyObjectByType<PlayerMovement>(); // Get the PlayerMovement script from the player object
        deliveryInteractionPrompt.DOFade(0f, 0f); // Ensure the shop interaction prompt is hidden at the start
        deliveryInteractionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(0f, 0f); // Ensure the prompt text is hidden at the start
    }

    void Update()
    {
        if (PlayerInteractionCheck())
        {
            if (InputSystem.actions["Interact"].triggered)
            {
                lastPickupCount = currentLevelManager.GetPickupCount();
                if (lastPickupCount == 0) { GameManager.Instance.LevelEnd(); }
            }
        }
    }

    public bool PlayerInteractionCheck()
    {
        if (currentLevelManager.GetPickupCount() == lastPickupCount)
        {
            if (!isFaded)
            {
                deliveryInteractionPrompt.DOFade(0f, 0.3f); // Fade out the shop interaction prompt
                deliveryInteractionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(0f, 0.3f); // Fade out the prompt text
                isFaded = true;
            }
            return false;
        }
        if (Vector2.Distance(playerMovement.transform.position, transform.position) <= 1f) // Check if the player is within interaction range
        {
            if (isFaded)
            {
                deliveryInteractionPrompt.DOFade(1f, 0.3f); // Fade in the shop interaction prompt
                deliveryInteractionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(1f, 0.3f); // Fade in the prompt text
                isFaded = false;
            }
            return true; // Player is close enough to interact with the shop
        }
        else
        {
            if (!isFaded)
            {
                deliveryInteractionPrompt.DOFade(0f, 0.3f); // Fade out the shop interaction prompt
                deliveryInteractionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(0f, 0.3f); // Fade out the prompt text
                isFaded = true;
            }
            return false; // Player is too far away to interact with the shop
        }

    }
}
