using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public AudioSource gameOverAudio; // Reference to the AudioSource component

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

        // Play game over audio
        if (gameOverAudio != null)
        {
            gameOverAudio.Play();
        }

        // Pause the game
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Resume time scale
        Time.timeScale = 1f;

        // Reset the player's health
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.ResetHealth();
        }

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToTitleScreen()
    {
        // Reset the player's health
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.ResetHealth();
        }

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
