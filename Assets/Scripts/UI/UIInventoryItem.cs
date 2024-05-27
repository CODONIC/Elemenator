using Inventory.Model;
using System;
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
        public InventoryItem CraftItem { get; private set; }

        private int itemIndex;
        [SerializeField] private int craftingIndex; // Modifiable in the inspector
        [SerializeField] public Image itemImage;
        [SerializeField] private TMP_Text quantityTxt;
        [SerializeField] private Image borderImage;

        public event Action<UIInventoryItem> OnItemClicked,
            OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
            OnRightMouseBtnClick;

        private bool empty = true;
        public CraftPanel theCraftPanel;

        private void Awake()
        {
            InitializeComponents();
            ResetData();
            Deselect();
        }

        private void Start()
        {
            theCraftPanel = FindObjectOfType<CraftPanel>();
        }

        private void InitializeComponents()
        {
            // Ensure all necessary components are properly assigned
            if (itemImage == null)
            {
                Debug.LogError("Item Image is not assigned for UIInventoryItem on GameObject: " + gameObject.name);
            }

            if (quantityTxt == null)
            {
                Debug.LogError("Quantity Text is not assigned for UIInventoryItem on GameObject: " + gameObject.name);
            }

            if (borderImage == null)
            {
                Debug.LogError("Border Image is not assigned for UIInventoryItem on GameObject: " + gameObject.name);
            }
        }

        public void SetItemIndex(int index)
        {
            itemIndex = index;
        }

        public int GetItemIndex()
        {
            return itemIndex;
        }

        public void SetCraftingIndex(int index)
        {
            craftingIndex = index;
        }

        public int GetCraftingIndex()
        {
            return craftingIndex;
        }

        public void ResetData()
        {
            InventoryItem = new InventoryItem();
            CraftItem = new InventoryItem();
            UpdateUI();
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
            InventoryItem = inventoryItem;
            empty = inventoryItem.IsEmpty;
            UpdateUI();
        }

        public void SetCraftData(InventoryItem craftItem)
        {
            CraftItem = craftItem;
            empty = craftItem.IsEmpty;
            UpdateUI();
        }


        private void UpdateUI()
        {
            if (itemImage == null || quantityTxt == null)
            {
                Debug.LogError("itemImage or quantityTxt is null.");
                return;
            }

            if (empty || (InventoryItem.IsEmpty && CraftItem.IsEmpty))
            {
                // Hide the item image and clear the quantity text when the item is empty
                itemImage.gameObject.SetActive(false);
                quantityTxt.text = string.Empty;
            }
            else
            {
                // Show the item image and update it based on whether it's from the inventory or crafting
                itemImage.gameObject.SetActive(true);
                itemImage.sprite = (!InventoryItem.IsEmpty && InventoryItem.item != null) ? InventoryItem.item.itemImage :
                   ((!CraftItem.IsEmpty && CraftItem.item != null) ? CraftItem.item.itemImage : null);

                // Update the quantity text based on which item is present (inventory or crafting)
                quantityTxt.text = (!InventoryItem.IsEmpty ? InventoryItem.quantity : CraftItem.quantity).ToString();
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
                        Debug.Log("Item clicked. ItemSO ID: " + GetItemSOID());
                    }
                    else
                    {
                        theCraftPanel.PickItemForCraft(this);

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
            if (!InventoryItem.IsEmpty && InventoryItem.item != null)
            {
                return InventoryItem.item.ID;
            }
            else
            {
                return -1;
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

        public void ClearCraftItem()
        {
            CraftItem = new InventoryItem
            {
                ID = -1,
                item = null,
                quantity = 0
            };
            UpdateUI();
            Debug.Log("CraftItem cleared.");
        }
    }
}