using UnityEngine;
using UnityEngine.UI;
using Inventory.Model; // Import the namespace of your InventorySO class

public class ResetButton : MonoBehaviour
{
    public void OnResetHealthButtonClick()
    {
        SaveManager.Instance.DeletePlayerPreference("PlayerHealth");
        Debug.Log("Player health preference deleted!");

        SaveManager.Instance.DeletePlayerPreference("PlayerPositionX");
        SaveManager.Instance.DeletePlayerPreference("PlayerPositionY");
        SaveManager.Instance.DeletePlayerPreference("PlayerPositionZ");
        Debug.Log("Player position preferences deleted!");

        // Delete the inventory JSON file
        InventorySO.Instance.DeleteInventoryFile();
        Debug.Log("Inventory JSON file deleted!");

        Debug.Log("Inventory cleared!");
    }
}
