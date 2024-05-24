using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler,
        IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        public InventoryItem InventoryItem { get; private set; }
        private int itemIndex;
        public Image itemImage;
        [SerializeField] private TMP_Text quantityTxt;
        [SerializeField] private Image borderImage;

        public event Action<UIInventoryItem> OnItemClicked,
            OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
            OnRightMouseBtnClick;

        private bool empty = true;
        public CraftPanel theCraftPanel;

        private void Awake()
        {
            ResetData();
            Deselect();
        }

        private void Start()
        {
            theCraftPanel = FindObjectOfType<CraftPanel>();
        }

        public void SetItemIndex(int index)
        {
            itemIndex = index;
        }

        public int GetItemIndex()
        {
            return itemIndex;
        }

        public void ResetData()
        {
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(false);
                empty = true;
            }
        }

        public void Deselect()
        {
            if (borderImage != null)
            {
                borderImage.enabled = false;
            }
        }

        public void SetData(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem; // Set the InventoryItem property
            if (inventoryItem.IsEmpty)
            {
                ResetData(); // Reset UI elements if inventoryItem is empty
            }
            else
            {
                // Update UI elements with inventoryItem data
                if (itemImage != null)
                {
                    itemImage.gameObject.SetActive(true);
                    itemImage.sprite = inventoryItem.item.ItemImage;
                }

                if (quantityTxt != null)
                {
                    quantityTxt.text = inventoryItem.quantity.ToString();
                }

                empty = false;
            }
        }


        public void Select()
        {
            if (borderImage != null)
            {
                borderImage.enabled = true;
            }
        }

        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);

                if (InventoryItem.ID != -1)
                {
                    if (!theCraftPanel.crafting)
                    {
                        Debug.Log("Item clicked. InventoryItem: " + InventoryItem.ToString());
                        Debug.Log("Item clicked. ID: " + InventoryItem.ID + ", Quantity: " + InventoryItem.quantity);
                        Debug.Log("Item clicked. ItemSO ID: " + GetItemSOID()); // Log the ItemSO ID
                    }
                    else
                    {
                        theCraftPanel.PickItemForCraft(this.gameObject.GetComponent<UIInventoryItem>());
                        Debug.Log("CRAFTING");
                    }
                }
                else
                {
                    Debug.LogWarning("Selected item is empty or invalid.");
                }
            }
        }

        private int GetItemSOID()
        {
            if (InventoryItem.IsEmpty  && InventoryItem.item != null)
            {
                return InventoryItem.item.ID;
            }
            else
            {
                return -1; // Return -1 if ItemSO is null or InventoryItem is null
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty)
                return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // You can add dragging logic here if needed
        }
    }
}
