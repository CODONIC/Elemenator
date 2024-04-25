using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Inventory.Model
{
    

    [System.Serializable]
    public class SerializableInventoryData
    {
        public List<SerializableInventoryItem> inventoryItems;

        public SerializableInventoryData(List<InventoryItem> items)
        {
            inventoryItems = new List<SerializableInventoryItem>();
            foreach (var item in items)
            {
                inventoryItems.Add(new SerializableInventoryItem(item));
            }
        }
    }

    public class InventoryDatabase : MonoBehaviour
    {
        public string saveFileName = "inventory.json"; // Name of the JSON file

        public InventorySO inventoryData; // Reference to the inventory data

        // Method to save inventory data to JSON file
        public void SaveInventory()
        {
            if (inventoryData == null)
            {
                Debug.LogWarning("InventoryData is null. Cannot save.");
                return;
            }

            string json = JsonUtility.ToJson(new SerializableInventoryData(inventoryData.inventoryItems));
            File.WriteAllText(GetSavePath(), json);
            Debug.Log("Inventory saved to: " + GetSavePath());
        }




        public void LoadInventory()
        {
            string filePath = GetSavePath();
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Debug.Log("JSON Data: " + json);

                var serializedData = JsonUtility.FromJson<SerializableInventoryData>(json);
                Debug.Log("Deserialized Data: " + serializedData);


                if (serializedData != null)
                {
                    // Add the loaded items to the inventory directly using InventorySO's method
                    inventoryData.AddSavedItems(serializedData.inventoryItems);


                    Debug.Log("Inventory loaded from: " + filePath);
                }
                else
                {
                    Debug.LogWarning("Failed to deserialize inventory data from JSON.");
                }
            }
            else
            {
                Debug.LogWarning("No inventory data found at: " + filePath);
            }

        }

        // Method to clear the inventory
        public void ClearInventory()
        {
            inventoryData.Clear();
            Debug.Log("Inventory cleared!");
        }

        // Method to get the full save file path
        private string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, saveFileName);
        }

        // Method to find an ItemSO by its ID
        private ItemSO GetItemSOByID(string itemID)
        {
            ItemSO[] itemSOs = Resources.FindObjectsOfTypeAll<ItemSO>();
            foreach (var itemSO in itemSOs)
            {
                if (itemSO.ID.ToString() == itemID)
                {
                    return itemSO;
                }
            }
            return null;
        }
    }
}
