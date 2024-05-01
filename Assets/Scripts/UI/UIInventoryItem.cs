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

        // Method to set the associated InventoryItem
        public void SetInventoryItem(InventoryItem item)
        {
            InventoryItem = item;
            // You might also want to update the UI to display information about this item
        }
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text quantityTxt;

        [SerializeField]
        private Image borderImage;

        public event Action<UIInventoryItem> OnItemClicked,
            OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
            OnRightMouseBtnClick;

        private bool empty = true;

        private int itemIndex = -1;

        public void SetItemIndex(int index)
        {
            itemIndex = index;
        }

        public void Awake()
        {
            ResetData();
            Deselect();
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


        public void OnPointerClick(PointerEventData pointerData)
        {

            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
            else
            {
                // Invoke item click event
                OnItemClicked?.Invoke(this);
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

        }
    }
}
