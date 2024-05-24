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
        public GameObject craftButton;

        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private UIInventoryDescription itemDescription;

        [SerializeField]
        private MouseFollower mouseFollower;

        [SerializeField] private GameObject confirmationDialogPanel;


        List<UIInventoryItem> listofUIItems = new List<UIInventoryItem>();
        
        // List to store UI for crafting slots

      


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
            craftButton.SetActive(false);
        }



        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                uiItem.SetItemIndex(i);
                listofUIItems.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
            }

           
        }



        public void UpdateData(int itemIndex, InventoryItem inventoryItem)
        {
            if (itemIndex < listofUIItems.Count)
            {
                listofUIItems[itemIndex].SetData(inventoryItem);
            }
        }



        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {

        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            ResetDraggedItem();
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

            craftButton.SetActive(true);
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
            craftButton.SetActive(false);
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
                // Check if the dropped item is from the inventory
                int draggedItemIndex = draggedItem.GetItemIndex();
                if (draggedItemIndex != -1)
                {
                    // If it's from the inventory, swap the items between the inventory and crafting slots
                    int selectedItemIndex = listofUIItems.FindIndex(item => item == draggedItem);
                    OnSwapItems?.Invoke(selectedItemIndex, currentlyDraggedItemIndex);

                    
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