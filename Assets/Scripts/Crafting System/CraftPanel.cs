using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftPanel : MonoBehaviour
{
    public GameObject panelToToggle;
    public GameObject craftButton;
    public GameObject exitButton;
    public GameObject MixButton;
    public GameObject equipButton;
    public TextMeshProUGUI craftingNotificationText;

    [SerializeField] private Animator Brewing;

    public UIInventoryItem[] craftingSlots = new UIInventoryItem[2];
    public Image[] craftingSlotImages = new Image[2];
    public CraftingSystem craftingSystem;

    public bool crafting;

    private void Start()
    {
        // Initialize the crafting slots
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            craftingSlots[i].SetCraftingIndex(i);
        }

        // Initially hide the notification text
        if (craftingNotificationText != null)
        {
            craftingNotificationText.gameObject.SetActive(false);
        }
    }

    public void PickItemForCraft(UIInventoryItem thisItem)
    {
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            if (craftingSlots[i].CraftItem.IsEmpty)
            {
                // If the slot is empty, set the item with quantity 1
                InventoryItem newItem = CopyInventoryItem(thisItem.InventoryItem, 1);
                craftingSlots[i].SetCraftData(newItem);
                UpdateCraftingSlotUI(i);
                Debug.Log($"CraftItem for slot {i} populated: ID - {craftingSlots[i].CraftItem.ID}, Item - {craftingSlots[i].CraftItem.item.Name}, Quantity - {craftingSlots[i].CraftItem.quantity}");
                return;
            }
            else if (craftingSlots[i].CraftItem.ID == thisItem.InventoryItem.ID)
            {
                // If the slot already contains this item, increment the quantity if it doesn't exceed the total quantity in the inventory
                InventoryItem updatedItem = craftingSlots[i].CraftItem;
                if (updatedItem.quantity < thisItem.InventoryItem.quantity)
                {
                    updatedItem.quantity++;
                    craftingSlots[i].SetCraftData(updatedItem);
                    UpdateCraftingSlotUI(i);
                    Debug.Log($"CraftItem for slot {i} incremented: ID - {craftingSlots[i].CraftItem.ID}, Item - {craftingSlots[i].CraftItem.item.Name}, Quantity - {craftingSlots[i].CraftItem.quantity}");
                }
                else
                {
                    Debug.LogWarning("Cannot add more items. Reached the total quantity in the inventory.");
                }
                return;
            }
        }
        Debug.LogWarning("All crafting slots are already occupied or items don't match.");
    }

    private InventoryItem CopyInventoryItem(InventoryItem original, int quantity)
    {
        return new InventoryItem
        {
            ID = original.ID,
            item = original.item,
            quantity = quantity
        };
    }

    public void Reset()
    {
        foreach (var craftingSlot in craftingSlots)
        {
            craftingSlot.ClearCraftItem();
        }

        foreach (var slotImage in craftingSlotImages)
        {
            if (slotImage != null)
            {
                slotImage.sprite = null;
                slotImage.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateCraftingSlotUI(int slotIndex)
    {
        if (craftingSlots[slotIndex].CraftItem.IsEmpty)
        {
            craftingSlotImages[slotIndex].sprite = null;
            craftingSlotImages[slotIndex].gameObject.SetActive(false);
        }
        else
        {
            craftingSlotImages[slotIndex].sprite = craftingSlots[slotIndex].CraftItem.item.itemImage;
            craftingSlotImages[slotIndex].gameObject.SetActive(true);
        }
    }

    public void EquipItemFromInventory(int slotIndex)
    {
        // Check if the slot index is valid
        if (slotIndex < 0 || slotIndex >= craftingSlots.Length)
        {
            Debug.LogError("Invalid slot index.");
            return;
        }

        // Get the inventory item from the selected slot
        InventoryItem inventoryItem = InventorySO.Instance.GetItemAt(slotIndex);

        // Check if the inventory item is empty or null
        if (inventoryItem.IsEmpty)
        {
            Debug.LogWarning("Selected inventory slot is empty.");
            return;
        }

        // Equip the inventory item into the crafting slot
        craftingSlots[slotIndex].SetCraftData(CopyInventoryItem(inventoryItem, 1)); // Start with quantity 1
        UpdateCraftingSlotUI(slotIndex);

        Debug.Log($"Item equipped into slot {slotIndex}: ID - {inventoryItem.ID}, Item - {inventoryItem.item.Name}, Quantity - {inventoryItem.quantity}");
    }

    public void TogglePanel()
    {
        if (panelToToggle != null)
        {
            crafting = true;
            bool panelActive = panelToToggle.activeSelf;
            panelToToggle.SetActive(!panelActive); // Toggle panel state
            exitButton.SetActive(true);
            if (!crafting)
            {
                Reset(); // Call Reset when toggling off the panel
                craftingNotificationText.gameObject.SetActive(false);
            }
            if (craftButton != null)
            {
                craftButton.SetActive(false);
            }
            else
            {
                Debug.LogError("Craft button reference is null. Please assign the craft button to the script in the Unity editor.");
            }
            if (equipButton != null)
            {
                equipButton.SetActive(false); // Hide the equip button
            }
            else
            {
                Debug.LogError("Equip button reference is null. Please assign the equip button to the script in the Unity editor.");
            }
        }
        else
        {
            Debug.LogError("Panel to toggle reference is null. Please assign a panel to the script in the Unity editor.");
        }
    }

    public void ExitPanel()
    {
        crafting = false;
        if (exitButton != null)
        {
            exitButton.SetActive(false); // Disable the exit button
        }
        else
        {
            Debug.LogError("Exit button reference is null. Please assign the exit button to the script in the Unity editor.");
        }

        TogglePanel(); // Call TogglePanel to also disable the panel

        craftButton.SetActive(true);
        craftingNotificationText.gameObject.SetActive(false);
        if (equipButton != null)
        {
            equipButton.SetActive(true); // Show the equip button
        }
        else
        {
            Debug.LogError("Equip button reference is null. Please assign the equip button to the script in the Unity editor.");
        }
    }

    public void OnCraftButtonClicked()
    {
        if (craftingSystem == null)
        {
            Debug.LogError("CraftingSystem reference is null. Please assign it in the Unity editor.");
            return;
        }

        InventoryItem[] inputItems = new InventoryItem[craftingSlots.Length];
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            if (craftingSlots[i] == null || craftingSlots[i].CraftItem.IsEmpty)
            {
                Debug.LogError($"Crafting slot {i} or its CraftItem is null.");
                return;
            }

            inputItems[i] = craftingSlots[i].CraftItem;
        }

        CraftingSystem.CraftingResult craftingResult = craftingSystem.Craft(inputItems);
        if (craftingResult != null)
        {
            Debug.Log("Crafting successful! Crafted item: " + craftingResult.CraftedItem.Name);

            if (craftingNotificationText != null)
            {
                craftingNotificationText.text = "Crafted item: " + craftingResult.CraftedItem.Name;
                craftingNotificationText.gameObject.SetActive(true); // Show the notification text

            }

            // Play the mixing animation by name
            Animator brewingAnimator = Brewing.GetComponent<Animator>();

            // Check if the brewingAnimator is not null
            if (brewingAnimator != null)
            {
                // Play the "Mixing" animation
                brewingAnimator.Play("Mixing");
            }
            else
            {
                Debug.LogError("Animator component not found on the Brewing GameObject.");
            }
            // Add the crafted item directly to the inventory
            InventorySO.Instance.AddItem(craftingResult.CraftedItem, 1);

            // Reduce quantities of the used items in the inventory
            foreach (var usedIngredient in craftingResult.UsedIngredients)
            {
                InventorySO.Instance.RemoveItem(usedIngredient.item, usedIngredient.quantity);
            }

            Reset(); // Clear the crafting slots after successful crafting
        }
        else
        {
            Debug.LogWarning("Crafting failed. No matching recipe found.");

            if (craftingNotificationText != null)
            {
                craftingNotificationText.text = "Crafting failed. No matching recipe found.";
                craftingNotificationText.gameObject.SetActive(true); // Show the notification text

            }
        }
    }
}
