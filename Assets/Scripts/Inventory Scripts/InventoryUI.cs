using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

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

    

    private void Awake()
    {
        InitializeInventorySlots();
    }

    // Method to display the element info when the element image is clicked
    private void ShowElementInfo(Element element)
    {
        // Display the element info in the InfoPanel's texts
        // Replace Debug.Log with your code to update the InfoPanel's texts
        Debug.Log("Element Name: " + element.name);
        Debug.Log("Element Symbol: " + element.symbol);
        Debug.Log("Element Atomic Number: " + element.atomicNumber);
    }

    private void InitializeInventorySlots()
    {
        // Calculate the number of rows and columns for the grid
        int rows = 4; // You can adjust this value based on your preferred layout
        int columns = 5; // You can adjust this value based on your preferred layout

        // Calculate the size of each slot based on the number of rows and columns
        Vector2 slotSize = new Vector2(150f, 150f); // Increased size for better visibility

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

            // Get the RectTransform component from the slot GameObject
            RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
            // Check if the RectTransform component is null
            if (slotRectTransform == null)
            {
                Debug.LogWarning("RectTransform component is null for slot GameObject: " + slot.name);
                continue; // Skip to the next iteration
            }

            // Calculate the position of the slot
            float xPos = startX + col * slotSize.x;
            float yPos = startY - row * slotSize.y;
            Vector2 slotPosition = new Vector2(xPos, yPos);
            slotRectTransform.anchoredPosition = slotPosition;

            // Adjust the size of the inventory slot
            slotRectTransform.sizeDelta = slotSize;

            

            // Add the instantiated slot to the list of inventory slots
            inventorySlots.Add(slot);

           
        }

        // Iterate through each inventory slot and add click event listener
        foreach (var slot in inventorySlots)
        {
            // Find the GameObject that contains the element image
            GameObject elementImageObject = slot.transform.Find("ElementImage").gameObject;

            // Add a Button component to the element image GameObject
            Button elementButton = elementImageObject.AddComponent<Button>();

            // Add a click event listener to the button
            elementButton.onClick.AddListener(() =>
            {
                // Retrieve the Element component from the element image GameObject
                Element element = slot.GetComponent<InventorySlot>().element;
                // Check if the element is valid
                if (element != null)
                {
                    ShowElementInfo(element);
                }
            });
        }
    }

    // Update inventory slots when the player collects a new element
    public void UpdateInventorySlot(Element element)
    {
        // Ensure inventory slots are initialized
        if (inventorySlots.Count == 0)
        {
            Debug.LogWarning("Inventory slots are not initialized.");
            return;
        }
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

                // Check if the InventorySlot component is null
                if (inventorySlot == null)
                {
                    Debug.LogWarning("InventorySlot component is null for slot GameObject: " + slot.name);
                    continue; // Skip to the next iteration
                }

                // Check if the slot already contains an element of the same type
                if (inventorySlot.element != null && inventorySlot.element == element)
                {
                    // Increase the quantity of the existing element in the slot
                    inventorySlot.quantity++;
                    // Update the quantity text of the inventory slot
                    inventorySlot.UpdateQuantityText(); // Ensure that the quantity text is updated
                    return; // Exit the loop once quantity is updated
                }

                // Check if the slot is empty
                if (inventorySlot.element == null)
                {
                    // Assign the element to the slot
                    inventorySlot.element = element;
                    // Set the quantity of the element in the slot to 1
                    inventorySlot.quantity = 1;

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

                        // Get the RectTransform component from the slot GameObject
                        RectTransform slotRectTransform = slot.GetComponent<RectTransform>();

                        // Check if the RectTransform component is null
                        if (slotRectTransform == null)
                        {
                            Debug.LogWarning("RectTransform component is null for slot GameObject: " + slot.name);
                            continue; // Skip to the next iteration
                        }

                        // Adjust the size of the element image to be slightly smaller than the size of the inventory slot
                        float newSize = Mathf.Min(slotRectTransform.sizeDelta.x, slotRectTransform.sizeDelta.y) * 0.7f;
                        elementImage.rectTransform.sizeDelta = new Vector2(newSize, newSize);

                        // Set the position of the element image to overlap the slot's sprite
                        elementImage.rectTransform.localPosition = Vector2.zero;

                        // Ensure that the quantity text is rendered on top of the element image
                        inventorySlot.quantityText.transform.SetAsLastSibling();

                        
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
