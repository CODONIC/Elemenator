using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 3f; // Speed of the fireball
    public int damage = 10; // Damage inflicted on collision

    public Rigidbody2D rb; // Assign the Rigidbody2D component in the Unity Editor

    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not assigned in the Unity Editor!");
            return;
        }

        // Find the player object by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        // Calculate direction towards the player
        Vector2 direction = (playerObject.transform.position - transform.position).normalized;

        // Set the initial velocity of the fireball towards the player
        rb.velocity = direction * speed;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the fireball collides with an object tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Deal damage to the player
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // Destroy the fireball on collision with the player
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy")) // Check if the fireball hits something other than an enemy
        {
            // Destroy the fireball on collision with any other object
            Destroy(gameObject);
        }
    }
}
