using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Singleton instance of SaveManager
    public static SaveManager Instance;

    private void Awake()
    {
        // Ensure there's only one instance of SaveManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to save the game state
    public void SaveGame(int playerHealth)
    {
        // Save relevant game data using PlayerPrefs or any other saving method
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    // Method to load the game state
    public int LoadGame()
    {
        // Load relevant game data using PlayerPrefs or any other loading method
        int playerHealth = PlayerPrefs.GetInt("PlayerHealth", 100); // Default value is 100 if the key doesn't exist
        Debug.Log("Game Loaded!");
        return playerHealth;
    }

    public void DeletePlayerPreference(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            Debug.Log("Deleted player preference with key: " + key);
        }
        else
        {
            Debug.LogWarning("Player preference with key: " + key + " not found!");
        }
    }
}
