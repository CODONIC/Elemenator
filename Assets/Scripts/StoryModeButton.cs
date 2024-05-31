using UnityEngine;
using UnityEngine.UI;

public class StoryModeButton : MonoBehaviour
{
    public GameObject messagePanel;
    public Text messageText;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(DisplayMessage);
    }

    void DisplayMessage()
    {
        messagePanel.SetActive(true);
        messageText.text = "Sorry, Story Mode is under development.";

        // Disable the message panel after 3 seconds
        Invoke("DisableMessagePanel", 2f);
    }

    void DisableMessagePanel()
    {
        messagePanel.SetActive(false);
    }
}
