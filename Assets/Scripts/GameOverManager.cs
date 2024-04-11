using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    void Start()
    {
        // Ensure game over screen is deactivated at the start
        gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
     
        
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
        // Load the title screen scene
        SceneManager.LoadScene("Start Screen"); // Replace "TitleScreen" with the name of your title screen scene
    }
}
