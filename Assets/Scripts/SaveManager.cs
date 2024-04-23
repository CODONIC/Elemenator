using Inventory.Model;
using System.Collections.Generic;
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
    // Method to save the game state
    public void SaveGame(int playerHealth, Vector3 playerPosition, List<InventoryItem> inventoryItems)
    {
        // Save player's health and position
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetFloat("PlayerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPositionY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", playerPosition.z);

        // Save inventory data
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            // Serialize inventory item data and save it using PlayerPrefs or another method
            string key = "InventoryItem_" + i;
            string value = JsonUtility.ToJson(inventoryItems[i]);
            PlayerPrefs.SetString(key, value);
        }

        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    public PlayerData LoadGame()
    {
        // Load player health and position
        int playerHealth = PlayerPrefs.GetInt("PlayerHealth", 100);
        float playerPositionX = PlayerPrefs.GetFloat("PlayerPositionX", 0f);
        float playerPositionY = PlayerPrefs.GetFloat("PlayerPositionY", 0f);
        float playerPositionZ = PlayerPrefs.GetFloat("PlayerPositionZ", 0f);
        Vector3 playerPosition = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

        // Load inventory items
        List<InventoryItem> inventoryItems = new List<InventoryItem>();
        int index = 0;
        while (true)
        {
            string key = "InventoryItem_" + index;
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                InventoryItem item = JsonUtility.FromJson<InventoryItem>(json);
                inventoryItems.Add(item);
                index++;
            }
            else
            {
                break;
            }
        }

        Debug.Log("Game Loaded!");
        return new PlayerData(playerHealth, playerPosition, inventoryItems);
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
    public List<InventoryItem> inventoryItems;

    public PlayerData(int health, Vector3 position, List<InventoryItem> inventoryItems)
    {
        this.health = health;
        this.position = position;
        this.inventoryItems = inventoryItems;
    }
}

