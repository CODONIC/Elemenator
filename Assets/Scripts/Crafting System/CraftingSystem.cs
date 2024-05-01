using Inventory.Model;
using Inventory.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.Crafting
{
    public class CraftingSystem : MonoBehaviour, IDropHandler
    {
        public UIInventoryPage inventoryPage;
        public ItemDatabaseObject itemDatabase; // Reference to the item database

        public void OnDrop(PointerEventData eventData)
        {
            // Retrieve the dragged item from the event data
            UIInventoryItem draggedItem = eventData.pointerDrag.GetComponent<UIInventoryItem>();

            // Retrieve the crafting slot from the event data
            CraftingSlot craftingSlot = eventData.pointerEnter.GetComponent<CraftingSlot>();

            if (draggedItem != null && craftingSlot != null)
            {
                // Determine the slot index of the crafting slot
                int slotIndex = craftingSlot.slotIndex;

                // Handle crafting with the dragged item and slot index
                HandleCrafting(draggedItem, slotIndex);
            }
        }

        public void HandleCrafting(UIInventoryItem draggedItem, int slotIndex)
        {
            // Retrieve the inventory item associated with the dragged UIInventoryItem
            InventoryItem inventoryItem = draggedItem.InventoryItem;

            // Check if the inventory item is validw
            if (inventoryItem.IsEmpty)
            {
                Debug.LogWarning("Dragged item is empty or invalid.");
                return;
            }

            // Now you have access to the inventory item, you can use it for crafting or other actions
            Debug.Log("Crafting with inventory item: " + inventoryItem.item.Name);

            // Get the ItemSO associated with the dragged inventory item
            ItemSO itemToAdd = null;
            if (itemDatabase.GetItem.TryGetValue(inventoryItem.ID, out itemToAdd))
            {
                // Add the dragged item to the specified crafting slot
                inventoryPage.AddItemToCraftingSlot(slotIndex, itemToAdd);
            }
            else
            {
                Debug.LogError("Item not found in the database with ID: " + inventoryItem.ID);
            }

            // Clear the crafting slots
            inventoryPage.ClearCraftingSlots();
        }

        // Implement the crafting logic here
        private ItemSO CraftItems(ItemSO item1, ItemSO item2)
        {
            // Example: If item1 has ID 1 and item2 has ID 2, craft item with ID 3
            int craftedItemId = GetCraftedItemId(item1.ID, item2.ID);

            // Retrieve the crafted item from the item database
            ItemSO craftedItem = GetCraftedItemById(craftedItemId);

            return craftedItem;
        }

        // Implement the logic to check if the combination of items is valid for crafting
        private bool CanCraftItems(ItemSO item1, ItemSO item2)
        {
            // Implement logic to check if the combination of items is valid for crafting
            return false;
        }

        // Implement your crafting logic here to determine the ID of the crafted item based on the IDs of item1 and item2
        private int GetCraftedItemId(int item1Id, int item2Id)
        {
            // Placeholder logic; replace it with your actual crafting logic
            return item1Id + item2Id;
        }

        // Retrieve the crafted item from the item database based on its ID
        private ItemSO GetCraftedItemById(int itemId)
        {
            if (itemDatabase.GetItem.TryGetValue(itemId, out ItemSO craftedItem))
            {
                return craftedItem;
            }
            else
            {
                Debug.LogError("Crafted item not found in the database with ID: " + itemId);
                return null;
            }
        }
    }
}
