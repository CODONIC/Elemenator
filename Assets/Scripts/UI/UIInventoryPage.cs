using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem itemPrefab;

        [SerializeField]
        private GameObject trashContent;

        [SerializeField]
        private List<UICraftingSlot> craftingSlots;


        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private UIInventoryDescription itemDescription;

        [SerializeField]
        private MouseFollower mouseFollower;

        [SerializeField] private GameObject confirmationDialogPanel;


        List<UIInventoryItem> listofUIItems = new List<UIInventoryItem>();

        private int currentlyDraggedItemIndex = -1;

        private int selectedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging;
        public event Action<int, int> OnSwapItems;

        public event Action<int> OnItemDeleted;

        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        

        public void InitializeInventoryUI(int inventorysize)
        {
            for (int i = 0; i < inventorysize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                uiItem.SetItemIndex(i);
                listofUIItems.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;


            }

            // Subscribe to the drop event for crafting slots
            for (int i = 0; i < craftingSlots.Count; i++)
            {
                UICraftingSlot craftingSlot = craftingSlots[i];
                craftingSlot.gameObject.AddComponent<CanvasGroup>(); // Ensure the GameObject has a CanvasGroup component
                EventTrigger trigger = craftingSlot.gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.Drop };
                entry.callback.AddListener((data) => { OnDrop((PointerEventData)data); });
                trigger.triggers.Add(entry);
            }
        }


        public void UpdateData(int itemIndex,
            Sprite itemImage, int itemQuantity)
        {
            if (listofUIItems.Count > itemIndex)
            {
                listofUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }
        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {

        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            ResetDraggedItem();
        }
        public void OnItemDroppedOntoCraftingSlot(UIInventoryItem inventoryItem, UICraftingSlot craftingSlot)
        {
            // Remove the item from the inventory
            int itemIndex = inventoryItem.GetItemIndex();
            // Handle the removal of the item from the inventory here...

            // Update the inventory UI to reflect the removal of the item
            // For example, if you have a method to reset the item data:
            inventoryItem.ResetData();

            // Notify the crafting slot that an item has been dropped onto it
            craftingSlot.OnCraftingSlotDropped(inventoryItem);
        }
        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listofUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {

                return;

            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = listofUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listofUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;

            // Deselect the previously selected item
            if (selectedItemIndex != -1)
                listofUIItems[selectedItemIndex].Deselect();

            selectedItemIndex = index;

            // Select the currently clicked item
            inventoryItemUI.Select();

            OnDescriptionRequested?.Invoke(selectedItemIndex);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            itemDescription.ResetDescription();

            // Activate Trash Content game object
            if (trashContent != null)
                trashContent.SetActive(true);

            ResetSelection();
            Time.timeScale = 0;
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listofUIItems)
            {
                item.Deselect();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            // Deactivate Trash Content game object
            if (trashContent != null)
                trashContent.SetActive(false);
            ResetDraggedItem();
            Time.timeScale = 1;
        }
       public void OnDrop(PointerEventData eventData)
        {
    // Check if the dropped object is an inventory item
            UIInventoryItem draggedItem = eventData.pointerDrag.GetComponent<UIInventoryItem>();
            if (draggedItem != null)
            {
                // Check if the drop target is a crafting slot
                UICraftingSlot craftingSlot = eventData.pointerEnter.GetComponent<UICraftingSlot>();
                if (craftingSlot != null)
                {
                // Trigger the crafting action with the dragged item
                craftingSlot.OnCraftingSlotDropped(draggedItem);

                    OnItemDeleted?.Invoke(selectedItemIndex);
                    listofUIItems[selectedItemIndex].ResetData();
                    selectedItemIndex = -1;
                    itemDescription.ResetDescription();

                }
            }
          }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listofUIItems[itemIndex].Select();
        }

        internal void ResetAllItems()
        {
            foreach (var item in listofUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public void DeleteSelectedItem()
        {
            if (selectedItemIndex != -1)
            {
                // Show confirmation dialog
                ShowConfirmationDialog(selectedItemIndex);
            }
        }

        private void ShowConfirmationDialog(int itemIndex)
        {
            // Activate the confirmation dialog panel
            confirmationDialogPanel.SetActive(true);

            // Assuming you have a confirmation dialog UI panel with "Yes" and "No" buttons.
            // Attach "Yes" button to ConfirmDeletion and "No" button to CancelDeletion methods.
        }

        public void ConfirmDeletion()
        {
            OnItemDeleted?.Invoke(selectedItemIndex);
            listofUIItems[selectedItemIndex].ResetData();
            selectedItemIndex = -1;
            itemDescription.ResetDescription();
            confirmationDialogPanel.SetActive(false); // Deactivate the confirmation dialog panel after confirmation
        }

        public void CancelDeletion()
        {
            confirmationDialogPanel.SetActive(false); // Deactivate the confirmation dialog panel if cancelled
        }


    }
}