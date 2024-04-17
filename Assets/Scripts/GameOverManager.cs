using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    // Reference to the MenuButton script
    public MenuButton menuButton;

    void Start()
    {
        // Ensure game over screen is deactivated at the start
        gameOverScreen.SetActive(false);

        // Find the MenuButton script in the scene
        menuButton = FindObjectOfType<MenuButton>();
        if (menuButton == null)
        {
            Debug.LogError("MenuButton script not found in the scene!");
        }
    }

    public void GameOver()
    {
        // Show game over screen
        gameOverScreen.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Reload the current scene (assuming your game has only one scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Resume time scale
        Time.timeScale = 1f;
    }

    public void GoToTitleScreen()
    {
        // Call GoToMainMenu() method from the MenuButton script
        if (menuButton != null)
        {
            menuButton.GoToMainMenu();
        }
        else
        {
            Debug.LogError("MenuButton reference is null. Make sure it's assigned in the inspector or MenuButton script exists in the scene.");
        }
    }
}
