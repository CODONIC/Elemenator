using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public Inventory playerInventory;
   public InventoryUI inventoryUI; // Reference to the InventoryUI script

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindInventoryUI();
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the scene loaded event
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindInventoryUI(); // Call this method to find the Inventory Button whenever a new scene is loaded
    }
    void FindInventoryUI()
    {
        GameObject inventoryUIGameObject = GameObject.Find("InventoryButton"); // Replace "InventoryUI" with the actual name of your Inventory UI game object
        if (inventoryUIGameObject != null)
        {
            inventoryUI = inventoryUIGameObject.GetComponent<InventoryUI>();
            if (inventoryUI == null)
            {
                Debug.LogError("InventoryUI script not found on the InventoryUI game object!");
                enabled = false; // Disable the script if InventoryUI component is missing
            }
        }
        else
        {
            Debug.LogError("InventoryUI game object not found in the scene!");
            enabled = false; // Disable the script if InventoryUI game object is missing
        }
    }

    public void AddElement(Element element)
    {
        if (inventoryUI != null)
        {
            // Add the element to the player's inventory
            playerInventory.elements.Add(element);

            // Notify the inventory UI to update the inventory slots
            inventoryUI.UpdateInventorySlot(element);
        }
        else
        {
            Debug.LogWarning("InventoryUI reference is null. Make sure to assign or find it in the scene.");
        }
    }

    // Other methods...
}
