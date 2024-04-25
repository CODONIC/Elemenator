using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        // Save player's health and position
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetFloat("PlayerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPositionY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", playerPosition.z);

        // Save inventory data
        InventoryDatabase inventoryDatabase = FindObjectOfType<InventoryDatabase>();
        if (inventoryDatabase != null)
        {
            inventoryDatabase.SaveInventory();
            Debug.Log("Game Saved!");
        }
        else
        {
            Debug.LogWarning("No InventoryDatabase found in the scene!");
        }

        PlayerPrefs.Save();
    }

    // Method to load the game state
    public PlayerData LoadGame()
    {
        // Load player health and position
        int playerHealth = PlayerPrefs.GetInt("PlayerHealth", 100);
        float playerPositionX = PlayerPrefs.GetFloat("PlayerPositionX", 0f);
        float playerPositionY = PlayerPrefs.GetFloat("PlayerPositionY", 0f);
        float playerPositionZ = PlayerPrefs.GetFloat("PlayerPositionZ", 0f);
        Vector3 playerPosition = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

        // Load inventory data
        InventoryDatabase inventoryDatabase = FindObjectOfType<InventoryDatabase>();
        if (inventoryDatabase != null)
        {
            inventoryDatabase.LoadInventory();
            Debug.Log("Game Loaded!");
            // Retrieve inventory items from the database
            List<InventoryItem> inventoryItems = inventoryDatabase.inventoryData.inventoryItems;
            return new PlayerData(playerHealth, playerPosition, inventoryItems);
        }
        else
        {
            Debug.LogWarning("No InventoryDatabase found in the scene!");
            return new PlayerData(playerHealth, playerPosition, new List<InventoryItem>());
        }
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

    // Method to clear the inventory
    public void ClearInventory()
    {
        InventoryDatabase inventoryDatabase = FindObjectOfType<InventoryDatabase>();
        if (inventoryDatabase != null)
        {
            inventoryDatabase.ClearInventory();
        }
        else
        {
            Debug.LogWarning("No InventoryDatabase found in the scene!");
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
