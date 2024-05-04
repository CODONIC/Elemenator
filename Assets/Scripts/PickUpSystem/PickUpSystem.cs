using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            // Call AddItem without assigning its return value
            inventoryData.AddItem(item.InventoryItem, item.Quantity);
            // Destroy the item regardless of the return value
            item.DestroyItem();
        }
    }
}
