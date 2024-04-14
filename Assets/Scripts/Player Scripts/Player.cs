using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private void Awake()
    {
        // Ensure the Player GameObject persists between scenes
        DontDestroyOnLoad(gameObject);

        // Initialize references on scene load
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure references are initialized after scene load
        // Example: GetComponent<PlayerHealth>().Initialize();
        // Ensure references are initialized after scene load
        healthBar = FindObjectOfType<HealthBar>();
        if (healthBar == null)
        {
            Debug.LogError("HealthBar reference not found in the scene!");
        }

        gameOverScreen = GameObject.Find("GameOverPanel");
        if (gameOverScreen == null)
        {
            Debug.LogError("GameOverScreen reference not found in the scene!");
        }
        else
        {
            gameOverScreen.SetActive(false); // Ensure the game over screen is initially inactive
        }
    }

    void Start()
    {
        // Load player's health and position from saved data
        PlayerData playerData = SaveManager.Instance.LoadGame();
        currentHealth = playerData.health;
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
            currentHealth = 0;
            Die();
        }
        else
        {
            // Start the flash effect coroutine
            StartCoroutine(FlashSprite());
        }
       
        

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
