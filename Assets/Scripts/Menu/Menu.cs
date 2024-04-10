using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menuPanel; // Reference to the UI panel to be shown

    // Start is called before the first frame update
    void Start()
    {
        // Ensure menuPanel is initially hidden when the game starts
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can add additional functionality here if needed
    }

    // Method to toggle the visibility of the menu panel
    public void ToggleMenuPanel()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
    }

    // Method to be called when the menu button is clicked
    public void OnMenuButtonClick()
    {
        ToggleMenuPanel(); // Toggle the menu panel visibility
    }
}
