using Inventory.Crafting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class CraftingSlot : MonoBehaviour, IDropHandler
    {
        public int slotIndex;
        private CraftingSystem craftingSystem;

        private void Start()
        {
            craftingSystem = FindObjectOfType<CraftingSystem>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            // Check if the dragged item is from the inventory
            if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<UIInventoryItem>() != null)
            {
                UIInventoryItem draggedItem = eventData.pointerDrag.GetComponent<UIInventoryItem>();

                // Pass the dragged item and the slot index to the crafting system for handling
                craftingSystem.HandleCrafting(draggedItem, slotIndex);
            }
        }
    }
}
