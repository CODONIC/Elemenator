using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraftPanel : MonoBehaviour
{
    public GameObject panelToToggle;
    public GameObject craftButton; // Reference to the craft button
    public GameObject exitButton;

    public UIInventoryItem firstItem;
    public UIInventoryItem secondItem;
    public Image fItem;
    public Image sItem;

    public bool crafting;

    public void PickItemForCraft (UIInventoryItem thisItem)
    {
        if(firstItem == null)
        {
            firstItem = thisItem;
            fItem.sprite = firstItem.itemImage.sprite;
            fItem.gameObject.SetActive(true);
            
        } else if (firstItem != null && secondItem == null)
        {
            secondItem = thisItem;
            sItem.sprite = secondItem.itemImage.sprite;
            sItem.gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        firstItem = null;
        secondItem = null;
        fItem.sprite = null;
        fItem.gameObject.SetActive(false);
        sItem.sprite = null;
        sItem.gameObject.SetActive(false);
    }

    public void TogglePanel()
    {
        if (panelToToggle != null)
        {
            crafting = true;
            bool panelActive = panelToToggle.activeSelf;
            panelToToggle.SetActive(!panelActive); // Toggle panel state
            exitButton.SetActive(true);

            if (craftButton != null)
            {
                craftButton.SetActive(false);

            }
            else
            {
                Debug.LogError("Craft button reference is null. Please assign the craft button to the script in the Unity editor.");
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
    }
}
