using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject inventorySlotPrefab; // Reference to the inventory slot prefab
    public Inventory playerInventory; // Reference to the player's inventory scriptable object

    private List<GameObject> inventorySlots = new List<GameObject>();

    private void Start()
    {
        InitializeInventorySlots();
    }

    private void InitializeInventorySlots()
    {
        // Calculate the number of rows and columns for the grid
        int rows = 4; // You can adjust this value based on your preferred layout
        int columns = 5; // You can adjust this value based on your preferred layout

        // Calculate the size of each slot based on the number of rows and columns
        Vector2 slotSize = new Vector2(100f, 100f); // Set the size of each slot based on your preference

        // Calculate the total width and height of the grid
        float totalWidth = columns * slotSize.x;
        float totalHeight = rows * slotSize.y;

        // Calculate the starting position of the first slot
        float startX = -totalWidth / 2f + slotSize.x / 2f;
        float startY = totalHeight / 2f - slotSize.y / 2f;

        // Clear existing inventory slots
        foreach (var slot in inventorySlots)
        {
            Destroy(slot);
        }
        inventorySlots.Clear();

        // Instantiate inventory slots
        for (int i = 0; i < rows * columns; i++)
        {
            // Calculate the row and column of the current slot
            int row = i / columns;
            int col = i % columns;

            // Instantiate a new inventory slot GameObject from the prefab
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel.transform);

            // Calculate the position of the slot
            float xPos = startX + col * slotSize.x;
            float yPos = startY - row * slotSize.y;
            Vector2 slotPosition = new Vector2(xPos, yPos);
            slot.GetComponent<RectTransform>().anchoredPosition = slotPosition;

            // Add the instantiated slot to the list of inventory slots
            inventorySlots.Add(slot);

            // You may need to adjust the size and layout of the inventory slot prefab.
        }
    }

    // Update inventory slots when the player collects a new element
    public void UpdateInventorySlot(Element element)
    {
        // Check if inventorySlots list is initialized and not null
        if (inventorySlots != null)
        {
            // Iterate over each slot in the inventorySlots list
            foreach (var slot in inventorySlots)
            {
                // Check if the slot GameObject is null
                if (slot == null)
                {
                    Debug.LogWarning("Slot GameObject is null.");
                    continue; // Skip to the next iteration
                }

                // Get the InventorySlot component from the slot GameObject
                var inventorySlot = slot.GetComponent<InventorySlot>();

                // Get the Image component from the slot GameObject
                var slotImage = slot.GetComponent<Image>();

                // Check if the Image component is null
                if (slotImage == null)
                {
                    Debug.LogWarning("Image component is null for slot GameObject: " + slot.name);
                    continue; // Skip to the next iteration
                }

                // Check if the slot already contains an element
                if (inventorySlot.element == null)
                {
                    // Assign the element to the slot
                    inventorySlot.element = element;

                    // Check if the collected element has a valid sprite assigned to it
                    if (element != null && element.image != null)
                    {
                        // Create a new GameObject for the element image
                        GameObject elementImageObject = new GameObject("ElementImage");

                        // Set the parent of the element image to the slot GameObject
                        elementImageObject.transform.SetParent(slot.transform, false);

                        // Add Image component to the element image GameObject
                        Image elementImage = elementImageObject.AddComponent<Image>();

                        // Set the sprite of the element image component with the sprite of the collected element
                        elementImage.sprite = element.image;

                        // Adjust the size of the element image to be slightly smaller than the size of the inventory slot
                        float newSize = Mathf.Min(slotImage.rectTransform.sizeDelta.x, slotImage.rectTransform.sizeDelta.y) * 0.7f;
                        elementImage.rectTransform.sizeDelta = new Vector2(newSize, newSize);

                        // Set the position of the element image to overlap the slot's sprite
                        elementImage.rectTransform.localPosition = Vector2.zero;
                    }
                    else
                    {
                        Debug.LogWarning("Element or element image is null for slot GameObject: " + slot.name);
                    }

                    return; // Exit the loop once element is assigned
                }
            }

            Debug.LogWarning("No available slots to store the element.");
        }
        else
        {
            Debug.LogWarning("InventorySlots list is null.");
        }
    }






    public void ToggleInventoryPanel()
    {
        bool inventoryActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(inventoryActive);

        if (inventoryActive)
        {
            PauseGame(); // Pause the game when inventory is opened
        }
        else
        {
            ResumeGame(); // Resume the game when inventory is closed
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
    }

    public void GoBack()
    {
        inventoryPanel.SetActive(false); // Hide the inventory panel
        Time.timeScale = 1f; // Resume the game
    }
}
