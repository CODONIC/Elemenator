using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UICraftingSlot : MonoBehaviour
    {
        public List<UIInventoryItem> craftingItems = new List<UIInventoryItem>();
        
        // Add references to UI components
        [SerializeField] private Image craftingSlotImage;
        [SerializeField] private Text craftingSlotQuantityText;

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
                // Assuming a quantity of 1 for now
                SetCraftingSlotData(itemSO.ItemImage, 1);
            }
        }

        private void SetCraftingSlotData(Sprite itemImage, int quantity)
        {
            // Implement the logic to set the image and quantity in the crafting slot UI
            if (craftingSlotImage != null)
            {
                craftingSlotImage.sprite = itemImage;
                craftingSlotImage.gameObject.SetActive(true);
            }

            if (craftingSlotQuantityText != null)
            {
                craftingSlotQuantityText.text = quantity.ToString();
            }
        }
    }
}
