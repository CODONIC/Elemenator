using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Newtonsoft.Json;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject, ISerializationCallbackReceiver
    {  
        public string savePath;
        private ItemDatabaseObject database;
      
        
        public List<InventoryItem> inventoryItems = new List<InventoryItem>();

        [SerializeField]
        private int size = 24;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public int Size => size;

        private void OnEnable()
        {
            database = Resources.Load<ItemDatabaseObject>("Database");
            if (database == null)
            {
                Debug.LogError("Failed to load ItemDatabaseObject. Make sure it's placed in a 'Resources' folder.");
            }
        }
        public void Initialize()
        {
            inventoryItems.Clear();

            

            // If the JSON file contains items, populate the inventory
            if (inventoryItems != null && inventoryItems.Count > 0)
            {
                
                    inventoryItems = inventoryItems.ToList();
                
            }
            else // If JSON file doesn't contain items, initialize the inventory as empty
            {
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
                string saveFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Save");

                if (!Directory.Exists(saveFolderPath))
                {
                    Directory.CreateDirectory(saveFolderPath);
                }

                // Combine the save folder path with the custom save path
                string filePath = Path.Combine(saveFolderPath, savePath);

                // Serialize the inventory data only
                InventorySaveData saveData = new InventorySaveData
                {
                    inventoryItems = inventoryItems
                };
                string saveJson = JsonUtility.ToJson(saveData, true);

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
                string saveFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Save");
                if (!Directory.Exists(saveFolderPath))
                {
                    Directory.CreateDirectory(saveFolderPath);
                }

                string filePath = Path.Combine(saveFolderPath, savePath);

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    Debug.Log("Loaded JSON data: " + json);

                    // Deserialize the JSON data into a list of InventoryItem objects
                    List<InventoryItem> loadedItems = JsonUtility.FromJson <List<InventoryItem>>(json);
                    Debug.Log("Loaded FROM JSON data: " + loadedItems);

                    if (loadedItems != null)
                    {
                        Debug.Log("Loaded Items FROM JSON data: " + loadedItems);
                        // Clear the existing inventoryItems list
                        inventoryItems.Clear();

                        // Add the loaded items to the inventoryItems list
                        inventoryItems.AddRange(loadedItems);

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
                Debug.LogError("Error loading game: " + ex.Message);
            }
        }



        public int AddItem(ItemSO item, int quantity)
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
                return quantity;
            }
            else
            {
                quantity = AddStackableItem(itemId, item, quantity);
                InformAboutChange();
                return quantity;
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

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
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
                        inventoryItems[i] = InventoryItem.GetEmptyItem();
                        quantity -= inventoryItems[i].quantity;
                    }
                    else
                    {
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
            return inventoryItems[itemIndex];
        }

        public void AddItem(InventoryItem item)
        {
            ItemSO itemToAdd;
            if (database.GetItem.TryGetValue(item.ID, out itemToAdd))
            {
                AddItem(itemToAdd, item.quantity);
            }
            else
            {
                Debug.LogError("Item not found in the database with ID: " + item.ID);
            }
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItem item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();
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
                inventoryItems = new List
             <InventoryItem>();
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

            // Filter out items with invalid IDs (-1)
            inventoryItems = inventoryItems.Where(item => item.ID >= 0).ToList();

            // Iterate over each item in the deserialized list and ensure it is valid
            for (int i = 0; i < inventoryItems.Count; i++)
            {
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
        public List<InventoryItem> inventoryItems;
    }
    [Serializable]
    public struct InventoryItem 
    {
        private ItemDatabaseObject database;

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
