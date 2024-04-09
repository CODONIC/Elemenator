using UnityEngine;

public class ElementPickup : MonoBehaviour
{
    // Reference to the Element scriptable object attached to the dropped element
    public Element element;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered");
        // Check if the player has collided with the element
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            // Get the InventoryManager component from the player object
            InventoryManager inventoryManager = other.GetComponent<InventoryManager>();

            // Check if the InventoryManager component exists
            if (inventoryManager != null)
            {
                Debug.Log("InventoryManager found");
                // Add the element to the player's inventory
                inventoryManager.AddElement(element);

                // Destroy the element object
                Destroy(gameObject);
            }
        }
    }
}
