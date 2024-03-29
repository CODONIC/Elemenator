using UnityEngine;

public class Player : MonoBehaviour
{
    public float startingHealth = 100f; // Initial health value
    private float currentHealth;        // Current health value

    public HealthBar healthBar;         // Reference to the health bar script

    void Start()
    {
        currentHealth = startingHealth; // Set current health to starting health
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth); // Update health bar with current health
        }
    }

    // Method to apply damage to the player
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Reduce current health by damage amount
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth); // Update health bar with new health
        }
        // Check if player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle player death
    void Die()
    {
        // Implement player death behavior here
        Debug.Log("Player died!");
    }
}
