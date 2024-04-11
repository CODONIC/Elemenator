using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    // Reference to the game panel
    public GameObject gamePanel;

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
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }

    // Method to save the game state using SaveManager
    // Method to save the game state using SaveManager
    // Method to save the game state using SaveManager
    public void SaveGame()
    {
        // Find the player GameObject by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Ensure playerObject is not null
        if (playerObject != null)
        {
            // Get the Player component from the player GameObject
            Player player = playerObject.GetComponent<Player>();

            // Ensure player component is not null
            if (player != null)
            {
                // Call SaveGame method from SaveManager and pass the current player health
                SaveManager.Instance.SaveGame(player.currentHealth);
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
