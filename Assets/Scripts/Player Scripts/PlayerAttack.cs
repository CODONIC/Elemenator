using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damageAmount = 10; // Damage amount to deal to the slime

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
            }
        }
    }
}
