using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;
    // This method is called when the user taps on the screen
    public void OnScreenTapped()
    {


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
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
