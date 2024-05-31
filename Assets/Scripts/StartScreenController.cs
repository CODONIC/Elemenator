using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    public Animator transition;
    public AudioSource backgroundMusic; // Reference to the background music audio source

    public float transitionTime = 1f;

    // This method is called when the user taps on the screen
    public void OnScreenTapped()
    {
        // Stop the background music
        StopBackgroundMusic();

        LoadNextLevel();
    }

    // Method to load a specific scene based on player choice
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(SceneTransition(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator SceneTransition(int levelIndex)
    {
        // Play the transition animation
        transition.SetTrigger("Start");

        // Wait for the transition animation to complete
        yield return new WaitForSeconds(transitionTime);

        // Load the next scene
        SceneManager.LoadScene(levelIndex);
    }

    private void Start()
    {
        // Play the background music
        backgroundMusic.Play();
    }

    private void StopBackgroundMusic()
    {
        // Stop the background music
        backgroundMusic.Stop();
    }
}
