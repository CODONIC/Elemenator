using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        public List<InventoryItem> inventoryItems;

        [field: SerializeField]
        public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        // Add method to add items directly from saved data
        public void AddSavedItems(List<SerializableInventoryItem> savedItems)
        {
            if (savedItems == null)
            {
                Debug.LogWarning("Saved items list is null.");
                return;
            }

            foreach (var savedItem in savedItems)
            {
                Debug.Log("Processing saved item: " + savedItem.itemID);

                ItemSO itemSO = GetItemSOByID(savedItem.itemID);
                if (itemSO != null)
                {
                    Debug.Log("ItemSO found for ID: " + savedItem.itemID);
                    int remainingQuantity = AddItem(itemSO, savedItem.quantity);
                    if (remainingQuantity > 0)
                    {
                        Debug.LogWarning("Failed to add all items with ID: " + savedItem.itemID + ". Remaining quantity: " + remainingQuantity);
                    }
                }
                else
                {
                    Debug.LogWarning("Failed to load item with ID: " + savedItem.itemID);
                }
            }
            InformAboutChange(); // Move InformAboutChange outside the loop to inform about inventory change after processing all items
        }



        public int AddItem(ItemSO item, int quantity)
        {
            

            if (item.IsStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while (quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1);
                    }
                    InformAboutChange();
                }
                return quantity; // Moved outside of the loop
            }
            quantity = AddStackableItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem
            {
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

        private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;

        public void RemoveItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].item == item)
                {
                    if (quantity >= inventoryItems[i].quantity)
                    {
                        // Remove the entire stack
                        inventoryItems[i] = InventoryItem.GetEmptyItem();
                        quantity -= inventoryItems[i].quantity;
                    }
                    else
                    {
                        // Reduce the quantity of the stack
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
            Initialize(); // Reinitialize the inventory after clearing
            InformAboutChange();
        }

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                if (inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
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
            AddItem(item.item, item.quantity);
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

    [System.Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSO item;

        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity,
            };
        }

        public static InventoryItem GetEmptyItem() => new InventoryItem
        {
            item = null,
            quantity = 0,
        };
    }

    [System.Serializable]
    public struct SerializableInventoryItem
    {
        public ItemSO inventoryItem; // Rename from InventoryItem to inventoryItem
        public int quantity;
        public string itemID; // Add itemID property

        public SerializableInventoryItem(InventoryItem item)
        {
            inventoryItem = item.item; // Assign item to inventoryItem
            quantity = item.quantity;
            itemID = inventoryItem != null ? inventoryItem.ID.ToString() : "-1"; // Populate itemID
        }
    }
}
