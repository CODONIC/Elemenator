using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this line to use TextMesh Pro

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TMP_Text healthText; // Change this line to use TMP_Text

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        UpdateHealthText(health); // Update the text
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        UpdateHealthText(health); // Update the text
    }

    private void UpdateHealthText(int health)
    {
        // Ensure health value is never less than 0
        int displayedHealth = Mathf.Max(health, 0);

        if (healthText != null)
        {
            healthText.text = $"HP: {displayedHealth}%";
        }
    }
}
