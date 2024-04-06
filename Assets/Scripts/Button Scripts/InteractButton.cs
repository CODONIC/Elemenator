using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    public GameObject interactButton;

    void Start()
    {
        if (interactButton == null)
        {
            Debug.LogError("Interact button reference not set!");
            enabled = false; // Disable the script if button reference is missing
        }
        else
        {
            interactButton.SetActive(false); // Ensure the button is initially inactive
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("Interactable")) // Check if the collider belongs to an NPC or a portal
        {
            // Show the interact button when the player enters the collider's trigger zone
            interactButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("Interactable")) // Check if the collider belongs to an NPC or a portal
        {
            // Hide the interact button when the player exits the collider's trigger zone
            interactButton.SetActive(false);
        }
    }
}
