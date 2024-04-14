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
    public void SaveGame(int playerHealth, Vector3 playerPosition)
    {
        // Save relevant game data using PlayerPrefs or any other saving method
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetFloat("PlayerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPositionY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", playerPosition.z);
        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    // Method to load the game state
    public PlayerData LoadGame()
    {
        // Load relevant game data using PlayerPrefs or any other loading method
        int playerHealth = PlayerPrefs.GetInt("PlayerHealth", 100); // Default value is 100 if the key doesn't exist
        float playerPositionX = PlayerPrefs.GetFloat("PlayerPositionX", 0f);
        float playerPositionY = PlayerPrefs.GetFloat("PlayerPositionY", 0f);
        float playerPositionZ = PlayerPrefs.GetFloat("PlayerPositionZ", 0f);
        Vector3 playerPosition = new Vector3(playerPositionX, playerPositionY, playerPositionZ);
        Debug.Log("Game Loaded!");
        return new PlayerData(playerHealth, playerPosition);
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

[System.Serializable]
public class PlayerData
{
    public int health;
    public Vector3 position;

    public PlayerData(int health, Vector3 position)
    {
        this.health = health;
        this.position = position;
    }
}
