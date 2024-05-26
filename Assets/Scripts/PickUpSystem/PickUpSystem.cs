using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

   
    [SerializeField]
    private TMP_Text collectedText;

    private void Start()
    {
        collectedText.enabled = false;
        collectedText.alpha = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            // Call AddItem without assigning its return value
            inventoryData.AddItem(item.InventoryItem, item.Quantity);
            // Destroy the item regardless of the return value
            item.DestroyItem();

            // Show collected text
            StartCoroutine(ShowCollectedText($"{item.InventoryItem.name} Collected!"));


        }


    }

    private IEnumerator ShowCollectedText(string message)
    {
        collectedText.text = message; // Set the text
        collectedText.enabled = true; // Make sure the text is enabled
        yield return StartCoroutine(FadeInText());
        yield return new WaitForSeconds(1f); // Wait for 1 second
        yield return StartCoroutine(FadeOutText());
        collectedText.enabled = false; // Hide the text
    }

    private IEnumerator FadeInText()
    {
        float duration = 0.5f; // Duration of fade-in
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            collectedText.alpha = alpha;
            collectedText.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, alpha); // Pop-out effect
            yield return null;
        }

        collectedText.alpha = 1f; // Ensure full visibility
        collectedText.transform.localScale = Vector3.one; // Ensure final scale
    }

    private IEnumerator FadeOutText()
    {
        float duration = 0.5f; // Duration of fade-out
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / duration));
            collectedText.alpha = alpha;
            collectedText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.8f, 1 - alpha); // Shrink effect
            yield return null;
        }

        collectedText.alpha = 0f; // Ensure invisibility
        collectedText.transform.localScale = Vector3.one * 0.8f; // Ensure initial scale
    }
}
