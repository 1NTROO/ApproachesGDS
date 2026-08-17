using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuInteract : MonoBehaviour
{
    [SerializeField] private Image interactionPrompt;
    private bool isFaded = true;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>(); // Get the PlayerMovement script from the player object
        interactionPrompt.DOFade(0f, 0f); // Ensure the shop interaction prompt is hidden at the start
        interactionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(0f, 0f); // Ensure the prompt text is hidden at the start
    }

    void Update()
    {
        if (PlayerInteractionCheck())
        {
            if (InputSystem.actions["Interact"].triggered)
            {
                Destroy(PlayerStatsManager.Instance.gameObject);
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public bool PlayerInteractionCheck()
    {
        if (Vector2.Distance(playerMovement.transform.position, transform.position) <= 3f) // Check if the player is within interaction range
        {
            if (isFaded)
            {
                interactionPrompt.DOFade(1f, 0.3f); // Fade in the shop interaction prompt
                interactionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(1f, 0.3f); // Fade in the prompt text
                isFaded = false;
            }
            return true; // Player is close enough to interact with the shop
        }
        else
        {
            if (!isFaded)
            {
                interactionPrompt.DOFade(0f, 0.3f); // Fade out the shop interaction prompt
                interactionPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(0f, 0.3f); // Fade out the prompt text
                isFaded = true;
            }
            return false; // Player is too far away to interact with the shop
        }

    }
}
