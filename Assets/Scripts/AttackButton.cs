using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public GameObject attackButton;

    void Start()
    {
        if (attackButton == null)
        {
            Debug.LogError("Attack button reference not set!");
            enabled = false; // Disable the script if button reference is missing
        }
        else
        {
            attackButton.SetActive(true); // Ensure the button is initially inactive
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("Portal")) // Check if the collider belongs to an NPC or a portal
        {
            // Show the attack button when the player enters the collider's trigger zone
            ToggleAttackButton(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("Portal")) // Check if the collider belongs to an NPC or a portal
        {
            // Hide the attack button when the player exits the collider's trigger zone
            ToggleAttackButton(true);
        }
    }

    void ToggleAttackButton(bool active)
    {
        if (attackButton != null)
        {
            attackButton.SetActive(active);
        }
    }
}
