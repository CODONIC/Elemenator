using Inventory.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    // Reference to the game panel
    public GameObject gamePanel;

    // Reference to the player's inventory
    [SerializeField] private InventorySO inventoryData;
    // Start is called before the first frame update
    void Start()
    {
        // Ensure the game panel is initially hidden
        if (gamePanel != null)
        {
            gamePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Game panel reference not set in MenuButton script!");
        }
    }

    // Method to handle continue button click
    public void OnMenuButtonClick()
    {
        // Toggle the game panel
        if (gamePanel != null)
        {
            bool panelActive = !gamePanel.activeSelf;
            gamePanel.SetActive(panelActive);

            // Pause or resume the game based on panel activity
            Time.timeScale = panelActive ? 0f : 1f;
        }
        else
        {
            Debug.LogError("Game panel reference not set in MenuButton script!");
        }

        // Add any other actions you want to perform when continuing the game
    }

    // Method to disable the panel
    public void DisablePanel()
    {
        if (gamePanel != null)
        {
            gamePanel.SetActive(false);
            Time.timeScale = 1f; // Ensure the game resumes when the panel is disabled
        }
        else
        {
            Debug.LogError("Game panel reference not set in MenuButton script!");
        }
    }

    // Method to go to the Main Menu scene
    public void GoToMainMenu()
    {
        SaveGame();

        // Find the player GameObject by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Check if playerObject is not null
        if (playerObject != null)
        {
            // Destroy the player GameObject
            Destroy(playerObject);
        }

        // Load the Main Menu scene
        SceneManager.LoadScene("Main Menu");

        // Resume the game when the Main Menu is loaded
        Time.timeScale = 1f;
    }

    // Method to save the game state using SaveManager
    public void SaveGame()
    {
        // Save player's health and position
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Player player = playerObject.GetComponent<Player>();
            if (player != null)
            {
                // Get player's health and position
                int playerHealth = player.currentHealth;
                Vector3 playerPosition = player.transform.position;

                // Save player's health and position
                SaveManager.Instance.SaveGame(playerHealth, playerPosition, inventoryData.inventoryItems);
            }
            else
            {
                Debug.LogError("Player component not found on the player GameObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
    }

    // Method to delete specific player preference
}
