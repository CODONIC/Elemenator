using UnityEngine;

public class Continue : MonoBehaviour
{
    // Reference to the game panel
    public GameObject gamePanel;

    // Reference to the UI panel
    public GameObject uiPanel;

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
            Debug.LogError("Game panel reference not set in Continue script!");
        }
    }

    // Method to handle continue button click
    public void OnContinueButtonClick()
    {
        // Show the game panel
        if (gamePanel != null)
        {
            gamePanel.SetActive(true);

            // Disable the UI panel
            if (uiPanel != null)
            {
                uiPanel.SetActive(false);
            }
            else
            {
                Debug.LogError("UI panel reference not set in Continue script!");
            }

            // Add any other actions you want to perform when continuing the game
        }
        else
        {
            Debug.LogError("Game panel reference not set in Continue script!");
        }
    }
}
