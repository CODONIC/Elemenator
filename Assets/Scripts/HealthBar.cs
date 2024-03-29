using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float maxHealth = 100f; // Max health value
    public Image healthBarImage;   // Reference to the health bar image component

    private float currentHealth;   // Current health value

    void Start()
    {
        currentHealth = maxHealth; // Set current health to max health at start
    }

    // Update the health bar based on the player's current health
    public void UpdateHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth); // Ensure health stays within range
        float healthPercentage = currentHealth / maxHealth;    // Calculate health percentage
        healthBarImage.fillAmount = healthPercentage;          // Update fill amount of health bar
    }
}
