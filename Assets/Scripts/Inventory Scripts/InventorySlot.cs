using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Element element; // Reference to the element in the slot
    public int quantity = 0; // Quantity of the element in the slot
    public InventoryUI inventoryUI;

    public TMP_Text quantityText; // Reference to the quantity text component

    // Method to update the slot with a new element
    public void AddElement(Element newElement)
    {
        element = newElement;
        quantity = 1; // Set initial quantity to 1
        UpdateQuantityText(); // Update the quantity text
    }

   

    // Method to update the quantity text
    public void UpdateQuantityText()
    {
        // Update the quantity text
        if (quantity > 1)
        {
            quantityText.text = quantity.ToString(); // Update the quantity text with the current quantity
            quantityText.gameObject.SetActive(true); // Show the quantity text
        }
        else
        {
            quantityText.gameObject.SetActive(false); // Hide the quantity text if quantity is 1 or less
        }

        // Ensure the text is rendered above the element image
        quantityText.transform.SetAsLastSibling();
    }

    // Method to clear the slot
    public void ClearSlot()
    {
        // Clear the slot's information
        element = null;
        quantity = 0;
        UpdateQuantityText(); // Update the quantity text after clearing the slot
    }

    // Method to handle clicking on the slot
    public void UseItem()
    {
        // Use the item (e.g., consume, equip, etc.)
        if (element != null)
        {
            Debug.Log("Using item: " + element.elementName);
        }
    }
}
