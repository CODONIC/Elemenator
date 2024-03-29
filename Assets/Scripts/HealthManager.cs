using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f; // Max health value
    private float currentHealth;   // Current health value
    public Image healthBarImage;   // Reference to the health bar image component

    void Start()
    {
        currentHealth = maxHealth; // Set current health to max health at start
        UpdateHealthBar();
    }

    // Method to update the health bar based on current health
    void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth;    // Calculate health percentage
        healthBarImage.fillAmount = healthPercentage;          // Update fill amount of health bar
    }

    // Method to apply damage to the player
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Reduce current health by damage amount
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Ensure health stays within range
        UpdateHealthBar(); // Update health bar display
    }

    // Method to restore health to the player
    public void Heal(float healAmount)
    {
        currentHealth += healAmount; // Increase current health by heal amount
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Ensure health stays within range
        UpdateHealthBar(); // Update health bar display
    }
}
