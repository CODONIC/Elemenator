using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int health = 50;
    public int damage = 10;
    public float attackRange = 1.5f;

    private Transform target;
    private Rigidbody2D rb;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target != null)
        {
            // Move towards the player
            Vector2 direction = (target.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

            // Attack if in range
            if (Vector2.Distance(transform.position, target.position) <= attackRange)
            {
                // Implement attack logic here
            }
        }
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
