using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindProjectile : MonoBehaviour
{
    public float speed;
    public int damage = 15;
    private Transform player;
    private Vector2 target;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Calculate the direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        // Set the target position to a point in the direction towards the player
        target = (Vector2)transform.position + direction * 25f; // Example: Move 25 units in the direction towards the player
        // Set initial rotation to face the player
        SetRotation(direction);
    }

    private void Update()
    {
        // Move the projectile towards the target
        Vector2 currentPosition = transform.position;
        Vector2 direction = (target - currentPosition).normalized;
        transform.position = Vector2.MoveTowards(currentPosition, target, speed * Time.deltaTime);

        // Rotate the projectile to face the direction of movement
        SetRotation(direction);

        // Define a threshold value for considering the projectile to have reached the target position
        float threshold = 0.25f; // Adjust this value as needed

        // Check if the distance between the projectile and its target position is less than the threshold
        if (Vector2.Distance(currentPosition, target) < threshold)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal damage to the player
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    void SetRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
