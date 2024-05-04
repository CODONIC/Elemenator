using Inventory.Model;
using Inventory.UI;
using UnityEngine;

namespace Inventory.Crafting
{
    public class CraftingSystem : MonoBehaviour
    {
        public UIInventoryPage inventoryPage;
        public ItemDatabaseObject itemDatabase; // Reference to the item database

        public void TransferItemToCraftingSlot(int slotIndex, InventoryItem item)
        {
            // Check if the item is valid
            if (item.item != null)
            {
                // Now you have access to the inventory item, you can use it for crafting or other actions
                Debug.Log("Crafting with inventory item: " + item.item.Name);

                // Get the ItemSO associated with the dragged inventory item
                ItemSO itemToAdd = null;
                if (itemDatabase.GetItem.TryGetValue(item.ID, out itemToAdd))
                {
                    // Add the dragged item to the specified crafting slot
                    inventoryPage.AddItemToCraftingSlot(slotIndex, itemToAdd);
                }
                else
                {
                    Debug.LogError("Item not found in the database with ID: " + item.ID);
                }

                // Clear the crafting slots
                inventoryPage.ClearCraftingSlots();
            }
            else
            {
                Debug.LogWarning("Dragged item is empty or invalid.");
            }
        }
    }
}
