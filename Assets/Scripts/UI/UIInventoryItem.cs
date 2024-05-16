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
        // Add a property to hold the associated InventoryItem
        public InventoryItem InventoryItem { get; private set; }
        private int itemIndex;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityTxt;
        [SerializeField] private Image borderImage;

        public event Action<UIInventoryItem> OnItemClicked,
            OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
            OnRightMouseBtnClick;

        private bool empty = true;
        public ItemSO ItemSO { get; private set; }
        private void Awake()
        {
            ResetData();
            Deselect();
        }

        public void SetInventoryItem(ItemSO item)
        {
            ItemSO = item;
            UpdateUI();
        }

       
        public void SetItemIndex(int index)
        {
            itemIndex = index;
        }

        public int GetItemIndex()
        {
            return itemIndex;
        }
        public InventoryItem GetInventoryItem()
        {
            // Here you should return the InventoryItem associated with this UIInventoryItem
            // For example, if you have a reference to the InventoryItem within UIInventoryItem, return it.
            return InventoryItem; // Placeholder, replace with the actual implementation
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

        public void SetData(Sprite sprite, int quantity)
        {
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(true);
                itemImage.sprite = sprite;
            }

            if (quantityTxt != null)
            {
                quantityTxt.text = quantity.ToString();
            }

            empty = false;
        }

        public void Select()
        {
            if (borderImage != null)
            {
                borderImage.enabled = true;
            }
        }

        public void UpdateUI()
        {
            // Implement UI update logic if needed
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
                    Debug.Log("Item clicked. InventoryItem: " + InventoryItem.ToString());
                    Debug.Log("Item clicked. ID: " + InventoryItem.ID + ", Quantity: " + InventoryItem.quantity);
                }
                else
                {
                    Debug.LogWarning("Selected item is empty or invalid.");
                }
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
