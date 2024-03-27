using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    private Button button;

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
            button.gameObject.SetActive(true); // Ensure the button is active
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal")) // Check if the collider belongs to the portal
        {
            // Hide the attack button when the player enters the portal's trigger zone
            button.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal")) // Check if the collider belongs to the portal
        {
            // Show the attack button when the player exits the portal's trigger zone
            button.gameObject.SetActive(true);
        }
    }
}
