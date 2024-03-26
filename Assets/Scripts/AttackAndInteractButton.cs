using UnityEngine;
using UnityEngine.UI;

public class AttackAndInteractButton : MonoBehaviour
{
    public Sprite defaultSprite; // Sprite for default state
    public Sprite activeSprite;  // Sprite for active state when collision detected

    private Button button;
    // Flag to track if interacting with NPC
    private Collider2D nearbyCollider; // Collider of the nearby NPC or interactable object

    void Start()
    {
        button = GameObject.FindWithTag("InteractButton").GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found!");
            enabled = false; // Disable the script if button component is missing
        }
        else
        {
            // Set the default sprite
            button.image.sprite = defaultSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to an NPC or interactable object
        if (other.CompareTag("NPC") || other.CompareTag("Interactable"))
        {
            nearbyCollider = other;

            // Change the sprite to the active sprite
            button.image.sprite = activeSprite;
        }
        else if (other.CompareTag("Portal")) // Check if the collider belongs to the portal
        {
            // Hide the attack button when the player enters the portal's trigger zone
            button.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider belongs to the nearby collider
        if (other == nearbyCollider)
        {
            // Change the sprite back to the default sprite
            button.image.sprite = defaultSprite;
            nearbyCollider = null;
        }
        else if (other.CompareTag("Portal")) // Check if the collider belongs to the portal
        {
            // Show the attack button when the player exits the portal's trigger zone
            button.gameObject.SetActive(true);
        }
    }



}
