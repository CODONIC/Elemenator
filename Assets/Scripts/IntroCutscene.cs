using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    public TMP_Text[] textElements;
    public Image[] imageElements;
    public Animator fadeAnimator;
    public AudioSource backgroundMusic; // Reference to the background music audio source
    public float charactersPerSecond = 50f; // Adjust typing speed as needed

    private int currentIndex = 0;
    private bool isTyping = false;

    private void Start()
    {
        // Hide all images and texts except the first one
        for (int i = 1; i < textElements.Length; i++)
        {
            textElements[i].gameObject.SetActive(false);
            imageElements[i].gameObject.SetActive(false);
        }

        // Play the background music
        backgroundMusic.Play();

        // Show the first image and text with typing animation
        ShowElementWithTypingAnimation(currentIndex);
    }

    public void Next()
    {
        if (!isTyping)
        {
            // Play fade out animation for the current image and text
            fadeAnimator.SetTrigger("FadeOut");

            // Wait for the fade out animation to complete
            StartCoroutine(ShowNextAfterFadeOut());
        }
    }

    private IEnumerator ShowNextAfterFadeOut()
    {
        // Wait for the end of the current frame before checking for the fade out animation completion
        yield return new WaitForEndOfFrame();

        // Get the length of the fade out animation from the Animator Controller
        float fadeOutDuration = fadeAnimator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the fade out animation to complete
        yield return new WaitForSeconds(fadeOutDuration);

        // Hide the current image and text
        imageElements[currentIndex].gameObject.SetActive(false);
        textElements[currentIndex].gameObject.SetActive(false);

        currentIndex++;
        if (currentIndex < textElements.Length)
        {
            // Show the next image and text with typing animation
            ShowElementWithTypingAnimation(currentIndex);

            // Trigger the fade-in animation
            fadeAnimator.SetTrigger("FadeIn");
        }
        else
        {
            // Stop the background music
            backgroundMusic.Stop();

            SceneManager.LoadScene("Unknown Island"); // End of cutscene, transition to gameplay or next scene
        }
    }

    private void ShowElementWithTypingAnimation(int index)
    {
        // Show the image
        imageElements[index].gameObject.SetActive(true);

        // Enable the text element
        textElements[index].gameObject.SetActive(true);

        // Start typing animation for the text
        StartCoroutine(TypeText(textElements[index]));
    }

    private IEnumerator TypeText(TMP_Text textElement)
    {
        isTyping = true;

        // Get the original text
        string originalText = textElement.text;

        // Clear the text
        textElement.text = "";

        // Type the text character by character
        float timePerCharacter = 1f / charactersPerSecond;
        foreach (char letter in originalText)
        {
            textElement.text += letter;
            yield return new WaitForSeconds(timePerCharacter);
        }

        // Show the text completely typed
        textElement.text = originalText;

        isTyping = false;
    }
}
