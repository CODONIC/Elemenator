using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using static UnityEditor.Progress;
using UnityEditor;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject, ISerializationCallbackReceiver
    {
        public string savePath;
        public ItemDatabaseObject database;

        public List<InventoryItem> inventoryItems = new List<InventoryItem>();

        [SerializeField]
        private int size = 20;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public int Size => size;

        // Static reference to hold the single instance of InventorySO
        private static InventorySO instance;


        public static InventorySO Instance
        {
            get
            {
                if (instance == null)
                {
                    // Load or create the InventorySO instance
                    instance = Resources.Load<InventorySO>("InventorySO");

                    // If the instance doesn't exist, create a new one
                    if (instance == null)
                    {
                        instance = CreateInstance<InventorySO>();
                        instance.Initialize();
                    }
                }
                return instance;
            }
        }

        private void OnEnable()
        {
            database = Resources.Load<ItemDatabaseObject>("Database");
            if (database == null)
            {
                Debug.LogError("Failed to load ItemDatabaseObject. Make sure it's placed in a 'Resources' folder.");
            }

            // Ensure the instance is set when the scriptable object is enabled
            instance = this;
        }

        public void Initialize()
        {
            try
            {
                Load(); // Try to load inventory data from the JSON file
                Debug.Log("Inventory initialized from saved data.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Failed to load inventory data from JSON file. Initializing with empty items. Error: " + ex.Message);
                // Clear the existing inventoryItems list
                inventoryItems.Clear();

                // Initialize with empty items
                for (int i = 0; i < Size; i++)
                {
                    inventoryItems.Add(InventoryItem.GetEmptyItem());
                }
            }
        }



        public void Save()
        {
            try
            {
                string saveFolderPath = Path.Combine(Application.persistentDataPath, "Save");

                if (!Directory.Exists(saveFolderPath))
                {
                    Directory.CreateDirectory(saveFolderPath);
                }

                // Combine the save folder path with the custom save path
                string filePath = Path.Combine(saveFolderPath, savePath);

                // Serialize the inventory data only
                InventorySaveData saveData = new InventorySaveData
                {
                    inventoryItems = inventoryItems.Select(item => new InventoryItemData
                    {
                        ID = item.ID,
                        quantity = item.quantity,
                        itemImagePath = item.item != null ? AssetDatabase.GetAssetPath(item.item) : null
                    }).ToList()
                };
                string saveJson = JsonConvert.SerializeObject(saveData, Formatting.Indented);

                // Write the JSON data to the file
                File.WriteAllText(filePath, saveJson);

                Debug.Log("Game saved successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error saving game: " + ex.Message);
            }
        }

        public void Load()
        {
            try
            {
                string saveFolderPath = Path.Combine(Application.persistentDataPath, "Save");
                if (!Directory.Exists(saveFolderPath))
                {
                    Directory.CreateDirectory(saveFolderPath);
                }

                string filePath = Path.Combine(saveFolderPath, savePath);

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    Debug.Log("Loaded JSON data: " + json);

                    // Deserialize the JSON data into a list of InventoryItemData objects
                    InventorySaveData saveData = JsonConvert.DeserializeObject<InventorySaveData>(json);

                    if (saveData != null)
                    {
                        // Clear the existing inventoryItems list
                        inventoryItems.Clear();

                        // Add the loaded items to the inventoryItems list
                        foreach (var data in saveData.inventoryItems)
                        {
                            // If the ID is -1, add an empty item
                            if (data.ID == -1)
                            {
                                inventoryItems.Add(InventoryItem.GetEmptyItem());
                            }
                            else
                            {
                                // Check if the item exists in the database
                                if (database.GetItem.TryGetValue(data.ID, out ItemSO item))
                                {
                                    inventoryItems.Add(new InventoryItem
                                    {
                                        ID = data.ID,
                                        quantity = data.quantity,
                                        item = item
                                    });
                                }
                                else
                                {
                                    Debug.LogWarning($"Item with ID {data.ID} not found in the database. Skipping...");
                                }
                            }
                        }

                        Debug.Log("Game loaded successfully.");
                        InformAboutChange();
                    }
                    else
                    {
                        Debug.LogError("Failed to deserialize JSON data into InventoryItem list.");
                    }
                }
                else
                {
                    Debug.Log("File Doesn't Exist");
                    Debug.Log(filePath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading data: " + ex.Message);
            }
        }


        public void AddItem(ItemSO item, int quantity)
        {
            int itemId = item.ID;

            if (!item.IsStackable)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while (quantity > 0 && !IsInventoryFull())
                    {
                        quantity -= AddItemToFirstFreeSlot(itemId, item, 1);
                    }
                    InformAboutChange();
                }
            }
            else
            {
                quantity = AddStackableItem(itemId, item, quantity);
                InformAboutChange();
            }

            // Call SetItemImage to ensure the sprite image is set for the added item
            SetItemImageForItem(item);
        }

        private void SetItemImageForItem(ItemSO item)
        {
            // Find the item in the inventory and set its sprite image
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].item == item)
                {
                    // Call the appropriate method to set the sprite image for the item
                    // For example, if your ItemSO has a method called SetItemImage, use it here
                    // inventoryItems[i].item.SetItemImage(item.ItemImage);
                    break;
                }
            }
        }

        private int AddItemToFirstFreeSlot(int itemId, ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem
            {
                ID = itemId,
                item = item,
                quantity = quantity
            };

            // Check if the inventory is completely empty
            if (inventoryItems.All(item => item.ID == -1))
            {
                inventoryItems[0] = newItem; // Add the item to the first slot
                return quantity;
            }

            // Look for the first available slot and add the item
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty || inventoryItems[i].ID == -1)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }

            // If no empty slots were found, add the item to a new slot
            inventoryItems.Add(newItem);
            return quantity;
        }



        private bool IsInventoryFull() => !inventoryItems.Any(item => item.IsEmpty);

        public void RemoveItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].item == item)
                {
                    if (quantity >= inventoryItems[i].quantity)
                    {
                        // Remove the entire item
                        inventoryItems[i] = InventoryItem.GetEmptyItem();
                        quantity -= inventoryItems[i].quantity;
                    }
                    else
                    {
                        // Decrease the quantity of the item
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity - quantity);
                        break;
                    }
                }
            }
            InformAboutChange();
        }


        public void Clear()
        {
            inventoryItems.Clear();
            Initialize();
            InformAboutChange();
        }

        private int AddStackableItem(int itemId, ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                if (inventoryItems[i].item.ID == itemId)
                {
                    int amountPossibleToTake = item.MaxStackSize - inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(item.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                        return 0;
                    }
                }
            }
            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(itemId, item, newQuantity);
            }
            return quantity;
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            // Check if the provided itemIndex is within the valid range
            if (itemIndex >= 0 && itemIndex < inventoryItems.Count)
            {
                return inventoryItems[itemIndex];
            }
            else
            {
                // If the index is out of range, return an empty item
                return InventoryItem.GetEmptyItem();
            }
        }


        public void AddItem(InventoryItem item)
        {
            // Add the item to the inventory
            AddItem(item.item, item.quantity);
        }




        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            // Check if the indices are within the range of inventoryItems
            if (itemIndex_1 < -1 || itemIndex_1 >= inventoryItems.Count ||
                itemIndex_2 < -1 || itemIndex_2 >= inventoryItems.Count)
            {
               
                return;
            }

            // If either slot is empty, treat it as an empty item
            InventoryItem item1 = itemIndex_1 < 0 ? InventoryItem.GetEmptyItem() : inventoryItems[itemIndex_1];
            InventoryItem item2 = itemIndex_2 < 0 ? InventoryItem.GetEmptyItem() : inventoryItems[itemIndex_2];

            // Perform the swap
            inventoryItems[itemIndex_1] = item2;
            inventoryItems[itemIndex_2] = item1;

            InformAboutChange();
        }

        public void DeleteInventoryFile()
        {
            try
            {
                string saveFolderPath = Path.Combine(Application.persistentDataPath, "Save");
                string filePath = Path.Combine(saveFolderPath, savePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log("Inventory JSON file deleted successfully." + filePath);
                }
                else
                {
                    Debug.Log("Inventory JSON file not found.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error deleting inventory JSON file: " + ex.Message);
            }
        }
        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            // Ensure that the inventoryItems list has the correct capacity
            if (inventoryItems == null)
            {
                inventoryItems = new List<InventoryItem>();
            }
            else
            {
                if (inventoryItems.Count > Size)
                {
                    inventoryItems.RemoveRange(Size, inventoryItems.Count - Size);
                }

                // Ensure that all items up to the specified size are initialized
                for (int i = inventoryItems.Count; i < Size; i++)
                {
                    inventoryItems.Add(InventoryItem.GetEmptyItem());
                }
            }

            // Iterate over each item in the deserialized list and ensure it is valid
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                // Skip items with invalid IDs (-1)
                if (inventoryItems[i].ID < 0)
                {
                    continue;
                }

                Debug.Log("Checking InventoryItem at index " + i + " with ID: " + inventoryItems[i].ID);
                ItemSO item;
                if (database.GetItem.TryGetValue(inventoryItems[i].ID, out item))
                {
                    Debug.Log("Item found in the database with ID: " + inventoryItems[i].ID);
                    // Create a new InventoryItem using the retrieved ItemSO
                    InventoryItem newItem = new InventoryItem
                    {
                        ID = inventoryItems[i].ID,
                        item = item,
                        quantity = inventoryItems[i].quantity
                    };
                    inventoryItems[i] = newItem;

                    Debug.Log("New InventoryItem at index " + i + ": " + inventoryItems[i]);
                }
                else
                {
                    Debug.LogWarning("Item not found in the database with ID: " + inventoryItems[i].ID);
                    // If the item is not found in the database, replace it with an empty item
                    inventoryItems[i] = InventoryItem.GetEmptyItem();
                }
            }
        }

    }



    [Serializable]
    public class InventorySaveData
    {
        public List<InventoryItemData> inventoryItems;
    }

    [Serializable]
    public struct InventoryItemData
    {
        public int ID;
        public int quantity;
        public string itemImagePath;
    }

    [Serializable]
    public struct InventoryItem
    {
        public int ID;
        public int quantity;
        public ItemSO item;

        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                ID = this.ID,
                item = this.item,
                quantity = newQuantity,
            };
        }

        public static InventoryItem GetEmptyItem()
        {
            return new InventoryItem
            {
                ID = -1,
                item = null,
                quantity = 0,
            };
        }
    }
}
