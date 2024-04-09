using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    public Inventory playerInventory;
    public InventoryUI inventoryUI; // Reference to the InventoryUI script

    public void AddElement(Element element)
    {
        // Add the element to the player's inventory
        playerInventory.elements.Add(element);

        // Notify the inventory UI to update the inventory slots
        inventoryUI.UpdateInventorySlot(element);
    }

    // Other methods...
}
