using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using Inventory.Model;
using Inventory.UI;
using Unity.VisualScripting;

namespace Crafting.UI
{
    public class UICraftingSlot : MonoBehaviour, IPointerClickHandler,
        IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        public InventoryItem CraftingItem { get; private set; }
        private int slotIndex;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityTxt;
        [SerializeField] private Image borderImage;

        [SerializeField]
        private InventorySO inventoryData;

        public event Action<UICraftingSlot> OnSlotClicked,
            OnSlotDroppedOn, OnSlotBeginDrag, OnSlotEndDrag,
            OnRightMouseBtnClick;

        private bool empty = true;
        public ItemSO ItemSO { get; private set; }

        private void Awake()
        {
            ResetData();
            Deselect();
        }

        public void SetCraftingItem(ItemSO item)
        {
            ItemSO = item;
            CraftingItem = new InventoryItem { ID = item.ID, item = item, quantity = 1 }; // Assuming a quantity of 1 for crafting
            UpdateUI();
        }

        public void SetSlotIndex(int index)
        {
            slotIndex = index;
        }

        public int GetSlotIndex()
        {
            return slotIndex;
        }

        public InventoryItem GetCraftingItem()
        {
            return CraftingItem;
        }

        public void ResetData()
        {
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(false);
                empty = true;
            }
            if (quantityTxt != null)
            {
                quantityTxt.text = string.Empty;
            }
            CraftingItem = InventoryItem.GetEmptyItem();
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
            if (ItemSO != null)
            {

                SetData(ItemSO.itemImage, CraftingItem.quantity);
            }
            else
            {
                ResetData();
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
                OnSlotClicked?.Invoke(this);

                if (CraftingItem.ID != -1)
                {
                    Debug.Log("Slot clicked. CraftingItem: " + CraftingItem.ToString());
                    Debug.Log("Slot clicked. ID: " + CraftingItem.ID + ", Quantity: " + CraftingItem.quantity);
                }
                else
                {
                    Debug.LogWarning("Selected slot is empty or invalid.");
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty)
                return;
            OnSlotBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnSlotEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var inventoryItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
                if (inventoryItemUI != null)
                {
                    ItemSO draggedItem = inventoryItemUI.ItemSO;
                    if (draggedItem != null)
                    {
                        SetCraftingItem(draggedItem);
                        OnSlotDroppedOn?.Invoke(this);
                    }
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            // You can add dragging logic here if needed
        }
    }
}
