using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public float flashDuration = 0.5f; // Duration of the flash effect
    public Color flashColor = Color.white; // Color to flash the sprite

    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component
    private Color originalColor; // Original color of the sprite

    public HealthBar healthBar;
    public GameObject gameOverScreen;

    void Start()
    {
        // Load player's health from saved data
        currentHealth = SaveManager.Instance.LoadGame();
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        // Get the SpriteRenderer component attached to the player object
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Store the original color of the sprite
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Start the flash effect coroutine
            StartCoroutine(FlashSprite());
        }
        // Save player's health after taking damage
        SaveManager.Instance.SaveGame(currentHealth);
    }

    IEnumerator FlashSprite()
    {
        Debug.Log("Flashing Sprite");

        // Set the sprite color to the flash color
        spriteRenderer.color = flashColor;

        // Wait for the specified duration
        yield return new WaitForSeconds(flashDuration);

        // Revert the sprite color back to its original color
        spriteRenderer.color = originalColor;

        Debug.Log("Sprite Color Reverted");
    }

    void Die()
    {
        Debug.Log("Player Died");
        // Activate game over screen
        gameOverScreen.SetActive(true);
        // Call the GameOver method in the GameOverManager script

        // Optionally, you can perform other actions here such as disabling player controls or animations.
    }
}
