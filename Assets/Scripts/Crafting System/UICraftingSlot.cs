using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;


namespace Inventory.UI
{
    public class UICraftingSlot : MonoBehaviour
    {
        // List to hold items in the crafting slot
        public List<UIInventoryItem> craftingItems = new List<UIInventoryItem>();

        public void OnCraftingSlotDropped(UIInventoryItem inventoryItem)
        {
            // Add the dropped item to the crafting slot list
            craftingItems.Add(inventoryItem);

            // Update the crafting slot UI to display the dragged item
            UpdateCraftingSlotUI(inventoryItem);
        }

        private void UpdateCraftingSlotUI(UIInventoryItem inventoryItem)
        {
            // Access the ItemSO associated with the UIInventoryItem
            ItemSO itemSO = inventoryItem.ItemSO;

            // Update the UI of the crafting slot with the itemSO data
            if (itemSO != null)
            {
                // Update the UI of the crafting slot with the item data
                inventoryItem.SetData(itemSO.ItemImage, 1); // Assuming a quantity of 1 for now
            }
        }
    }
}
