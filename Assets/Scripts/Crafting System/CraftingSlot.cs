using Inventory.Crafting;
using Inventory.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class CraftingSlot : MonoBehaviour
    {
        public int slotIndex;
        private CraftingSystem craftingSystem;

        private void Start()
        {
            craftingSystem = FindObjectOfType<CraftingSystem>();
        }

        public void OnCraftingSlotTap()
        {
            // Get the selected item from the inventory
            InventoryItem selectedItem = InventorySO.Instance.GetItemAt(0); // Assuming index 0 for the selected item

            // Pass the selected item and slot index to the crafting system for handling
            craftingSystem.TransferItemToCraftingSlot(slotIndex, selectedItem);
        }
    }
}
