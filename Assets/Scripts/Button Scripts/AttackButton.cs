using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AttackButton : MonoBehaviour
{
    public GameObject attackButton;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
        FindAttackButton(); // Call this method to find the attack button in the current scene
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the scene loaded event
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAttackButton(); // Call this method to find the attack button whenever a new scene is loaded
    }

    void FindAttackButton()
    {
        attackButton = GameObject.Find("Attack Button"); // Replace "AttackButton" with the actual name of your attack button game object
        if (attackButton == null)
        {
            Debug.LogError("Attack button reference not found in the scene!");
            enabled = false; // Disable the script if button reference is missing
        }
        else
        {
            attackButton.SetActive(true); // Ensure the button is initially active
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
