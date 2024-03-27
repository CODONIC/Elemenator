using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    private Button button;
    private Collider2D nearbyCollider; // Collider of the nearby NPC or interactable object

    void Start()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found!");
            enabled = false; // Disable the script if button component is missing
        }
        else
        {
            // Initially, hide the interact button
            button.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to an NPC or interactable object
        if (other.CompareTag("NPC") || other.CompareTag("Interactable"))
        {
            nearbyCollider = other;

            // Show the interact button
            button.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider belongs to the nearby collider
        if (other == nearbyCollider)
        {
            // Hide the interact button
            button.gameObject.SetActive(false);
            nearbyCollider = null;
        }
    }
}
