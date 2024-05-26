using Inventory.Model;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image itemImage; // Reference to the UI Image component to display the item
    public bool IsEmpty { get; private set; } = true;
    private InventoryItem equippedItem;

    public void EquipItem(InventoryItem item)
    {
        if ( item.IsEmpty)
        {
            Debug.LogError("Trying to equip an empty or null item.");
            return;
        }

        equippedItem = item;
        itemImage.sprite = item.item.itemImage;
        itemImage.gameObject.SetActive(true);
        IsEmpty = false;

        Debug.Log($"Equipped item: {item.item.Name}");
    }

    public void UnequipItem()
    {
      
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
        IsEmpty = true;

        Debug.Log("Item unequipped.");
    }
}
