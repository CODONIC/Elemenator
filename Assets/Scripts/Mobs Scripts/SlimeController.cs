using UnityEngine;
using System.Collections;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int health = 50;
    public int damage = 10;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;
    public float delayAfterCollision = 1f; // Delay in seconds after colliding with player

    private Transform target;
    private bool canMove = true; // Flag to control whether the slime can move
    private Collider2D detectionCollider; // Reference to the detection range collider

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Find the detection collider in the child object named "Detection"
        Transform detectionTransform = transform.Find("Detection");
        if (detectionTransform != null)
        {
            detectionCollider = detectionTransform.GetComponent<Collider2D>();
            if (detectionCollider == null)
            {
                Debug.LogError("Detection collider not found!");
            }
            else
            {
                detectionCollider.isTrigger = true; // Ensure the collider is set as a trigger
            }
        }
        else
        {
            Debug.LogError("Detection transform not found!");
        }
    }

    void Update()
    {
        if (target != null && canMove)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            // Check if the player is within detection range
            if (distanceToPlayer <= detectionRange)
            {
                // Move towards the player if within detection range
                Vector2 direction = (target.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);

                // Attack if in range
                if (distanceToPlayer <= attackRange)
                {
                    // Implement attack logic here
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage the player
            Player playerController = collision.gameObject.GetComponent<Player>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }

            // Start delay coroutine
            StartCoroutine(DelayBeforeChase());
        }
    }

    IEnumerator DelayBeforeChase()
    {
        canMove = false; // Disable movement
        yield return new WaitForSeconds(delayAfterCollision); // Wait for delay
        canMove = true; // Enable movement again
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Implement death behavior (e.g., play death animation, drop loot, etc.)
        Destroy(gameObject);
    }
}
