using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damageAmount = 10.0f; // Damage amount to deal to enemies
    public float knockbackForce = 5.0f; // Force to apply for knockback

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is an enemy (slime or firefly)
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out SlimeController slimeController))
            {
                // Deal damage to the slime
                slimeController.TakeDamage(damageAmount);

                // Apply knockback force to the slime
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                slimeController.ApplyKnockback(knockbackDirection, knockbackForce);
            }
            else if (other.TryGetComponent(out FireFlyControl fireflyController))
            {
                // Deal damage to the firefly
                fireflyController.TakeDamage(damageAmount);

                // Apply knockback force to the firefly
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                
            }
            else if (other.TryGetComponent(out AirFlyControl airflyController))
            {
                airflyController.TakeDamage(damageAmount);
            }
            else if (other.TryGetComponent(out ArachnidControl arachnidController))
            {
                arachnidController.TakeDamage(damageAmount);
            }
            else if (other.TryGetComponent(out AberrantControl aberrantController))
            {
                aberrantController.TakeDamage(damageAmount);
            }
            else if (other.TryGetComponent(out GolemnControl golemnController))
            {
                golemnController.TakeDamage(damageAmount);
            }
        }
    }
}
