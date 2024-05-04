using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damageAmount = 10.0f; // Damage amount to deal to the slime
    public float knockbackForce = 5.0f; // Force to apply for knockback

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the slime
        if (other.CompareTag("Enemy"))
        {
            // Retrieve the slime's SlimeController component
            SlimeController slimeController = other.GetComponent<SlimeController>();

            // Check if the slimeController is not null
            if (slimeController != null)
            {
                // Deal damage to the slime
                slimeController.TakeDamage(damageAmount);

                // Apply knockback force to the enemy
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                slimeController.ApplyKnockback(knockbackDirection, knockbackForce);
            }
        }
    }
}
