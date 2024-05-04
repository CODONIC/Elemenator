using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public float interactionRange = 3f; // Range within which the player can interact with the portal
    public GameObject teleportButton; // Reference to the teleport button GameObject

    private void Start()
    {
        // Initially hide the teleport button
        teleportButton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Show debug message
            Debug.Log("Player entered trigger zone");

            // Show the teleport button
            teleportButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Show debug message
            Debug.Log("Player exited trigger zone");

            // Hide the teleport button
            teleportButton.SetActive(false);
        }
    }

    public void TeleportPlayer()
    {
        // Load the desired scene
        SceneManager.LoadScene("Laboratory 2-1");
    }
}
