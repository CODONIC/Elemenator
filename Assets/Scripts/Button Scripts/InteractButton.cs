using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractButton : MonoBehaviour
{
    public GameObject interactButton;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
        FindInteractButton(); // Call this method to find the interact button in the current scene
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the scene loaded event
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindInteractButton(); // Call this method to find the interact button whenever a new scene is loaded
    }

    void FindInteractButton()
    {
        interactButton = GameObject.Find("Interact Button"); // Replace "InteractButton" with the actual name of your interact button game object
        if (interactButton == null)
        {
            Debug.LogError("Interact button reference not found in the scene!");
            enabled = false; // Disable the script if button reference is missing
        }
        else
        {
            interactButton.SetActive(false); // Ensure the button is initially inactive
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("Interactable")) // Check if the collider belongs to an NPC or an interactable object
        {
            // Show the interact button when the player enters the collider's trigger zone
            interactButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("Interactable")) // Check if the collider belongs to an NPC or an interactable object
        {
            // Hide the interact button when the player exits the collider's trigger zone
            interactButton.SetActive(false);
        }
    }
}
