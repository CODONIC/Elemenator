using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingResult : MonoBehaviour
{
    public ItemSO CraftedItem { get; set; }
    public List<InventoryItem> UsedIngredients { get; set; }
    public int CraftingSlotIndex { get; set; } // Add this property
}
