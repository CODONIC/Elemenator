using Inventory.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    // Singleton instance of SaveManager
    public static SaveManager Instance;

    public InventorySO inventory;
   

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
        // Save player's health and position
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetFloat("PlayerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPositionY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", playerPosition.z);

        inventory.Save();

      
            Debug.Log("Game Saved!");
        

        PlayerPrefs.Save();
    }

    // Method to load the game state
    public PlayerData LoadGame(InventorySO inventory)
    {
        // Load player health and position
        int playerHealth = PlayerPrefs.GetInt("PlayerHealth", 1000);
        float playerPositionX = PlayerPrefs.GetFloat("PlayerPositionX", 0f);
        float playerPositionY = PlayerPrefs.GetFloat("PlayerPositionY", 0f);
        float playerPositionZ = PlayerPrefs.GetFloat("PlayerPositionZ", 0f);
        Vector3 playerPosition = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

        // Create a new PlayerData instance with loaded data
        PlayerData playerData = new PlayerData(playerHealth, playerPosition);

        inventory.Load();
        Debug.Log("Game Loaded");
        return playerData;


    }


    // Method to delete a player preference
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
    [JsonIgnore] // Exclude the position property from serialization
    public Vector3 position;
    public List<InventoryItem> inventoryItems;

    public PlayerData(int health, Vector3 position)
    {
        this.health = health;
        this.position = position;
        
    }
}
