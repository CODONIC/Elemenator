using Inventory.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Crafting
{
    public class CraftingSlot : MonoBehaviour
    {
        public Image iconImage;
        private ItemSO currentItem;

        public void SetCraftingItem(ItemSO item)
        {
            currentItem = item;
            if (item != null)
            {
                iconImage.sprite = item.ItemImage;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }
        }

        public void OnClick()
        {
            if (currentItem != null)
            {
                Debug.Log("Crafting slot clicked. Item: " + currentItem.Name);
                // Implement crafting logic here
            }
            else
            {
                Debug.LogWarning("Crafting slot is empty.");
            }
        }
    }
}
