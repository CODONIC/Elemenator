using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using UnityEditor;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject, ISerializationCallbackReceiver
    {
        public string savePath;
        public ItemDatabaseObject database;

        public List<InventoryItem> inventoryItems = new List<InventoryItem>();
        public List<InventoryItem> craftingSlots = new List<InventoryItem>(); // List of crafting slots
        public InventoryItem resultSlot = InventoryItem.GetEmptyItem();

        [SerializeField]
        private int size = 24;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public int Size => size;

        private static InventorySO instance;

        public static InventorySO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<InventorySO>("InventorySO");

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

            instance = this;
        }

        public void Initialize()
        {
            try
            {
                Load();
                Debug.Log("Inventory initialized from saved data.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Failed to load inventory data from JSON file. Initializing with empty items. Error: " + ex.Message);
                inventoryItems.Clear();

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

                string filePath = Path.Combine(saveFolderPath, savePath);

                InventorySaveData saveData = new InventorySaveData
                {
                    inventoryItems = inventoryItems.Select(item => new InventoryItemData
                    {
                        ID = item.ID,
                        quantity = item.quantity,
                        itemImagePath = item.item != null ? item.item.itemImagePath : null
                    }).ToList()
                };
                string saveJson = JsonConvert.SerializeObject(saveData, Formatting.Indented);

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

                    InventorySaveData saveData = JsonConvert.DeserializeObject<InventorySaveData>(json);

                    if (saveData != null)
                    {
                        inventoryItems.Clear();

                        foreach (var data in saveData.inventoryItems)
                        {
                            if (data.ID == -1)
                            {
                                inventoryItems.Add(InventoryItem.GetEmptyItem());
                            }
                            else
                            {
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

            SetItemImageForItem(item);
        }

        private void SetItemImageForItem(ItemSO item)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].item == item)
                {
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

            if (inventoryItems.All(item => item.ID == -1))
            {
                inventoryItems[0] = newItem;
                return quantity;
            }

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty || inventoryItems[i].ID == -1)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }

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
            if (itemIndex >= 0 && itemIndex < inventoryItems.Count)
            {
                return inventoryItems[itemIndex];
            }
            else
            {
                return InventoryItem.GetEmptyItem();
            }
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            if (itemIndex_1 < -1 || itemIndex_1 >= inventoryItems.Count ||
                itemIndex_2 < -1 || itemIndex_2 >= inventoryItems.Count)
            {
                return;
            }

            InventoryItem item1 = itemIndex_1 < 0 ? InventoryItem.GetEmptyItem() : inventoryItems[itemIndex_1];
            InventoryItem item2 = itemIndex_2 < 0 ? InventoryItem.GetEmptyItem() : inventoryItems[itemIndex_2];

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

                for (int i = inventoryItems.Count; i < Size; i++)
                {
                    inventoryItems.Add(InventoryItem.GetEmptyItem());
                }
            }

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].ID < 0)
                {
                    continue;
                }

                Debug.Log("Checking InventoryItem at index " + i + " with ID: " + inventoryItems[i].ID);
                ItemSO item;
                if (database.GetItem.TryGetValue(inventoryItems[i].ID, out item))
                {
                    Debug.Log("Item found in the database with ID: " + inventoryItems[i].ID);
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
