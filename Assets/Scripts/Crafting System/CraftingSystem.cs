using Inventory.Model;
using UnityEngine;

namespace Inventory.Crafting
{
    public class CraftingSystem : MonoBehaviour
    {
        public static CraftingSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void CraftItem(ItemSO item)
        {
            // Implement crafting logic here
            Debug.Log("Crafting item: " + item.Name);
        }
    }
}
