using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    
 
    [SerializeField]
    private UIInventoryItem item;

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
      
        item = GetComponentInChildren<UIInventoryItem>();
    }

    public void SetData(Sprite sprite, int quantity)
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.item = new ItemSO(); // Create a new ItemSO instance, or get it from somewhere
        inventoryItem.item.SetItemImage(sprite); // Set the sprite to the ItemSO
        inventoryItem.quantity = quantity; // Set the quantity
        item.SetData(inventoryItem); // Pass the InventoryItem to UIInventoryItem's SetData method
    }

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
            );
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        Debug.Log($"Item toggled {val}");
        gameObject.SetActive(val);
    }

}
