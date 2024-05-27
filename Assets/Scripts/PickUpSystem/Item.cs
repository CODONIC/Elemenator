using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float duration = 0.3f;
    // Start is called before the first frame update

    [SerializeField]
    private float floatAmount = 1.5f;
    private Vector3 initialPosition;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
        initialPosition = transform.position;
        StartCoroutine(FloatAnimation());
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());

    }
    private IEnumerator FloatAnimation()
    {
        while (true)
        {
            float newY = initialPosition.y + Mathf.Sin(Time.time) * floatAmount;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
    }
    private IEnumerator AnimateItemPickup()
    {
        audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale =
                Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
