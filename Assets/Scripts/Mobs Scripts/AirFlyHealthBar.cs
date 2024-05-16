using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirFlyHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider

    private AirFlyControl AirflyController; // Reference to the FireflyController script

    void Start()
    {
        // Find and store a reference to the FireflyController script attached to the firefly
        AirflyController = GetComponent<AirFlyControl>();

        // Set the maximum value of the health slider to the initial health of the firefly
        healthSlider.maxValue = AirflyController.health;
        // Set the current value of the health slider to the initial health of the firefly
        healthSlider.value = AirflyController.health;
    }

    void Update()
    {
        // Check if fireflyController is not null before using it
        if (AirflyController != null)
        {
            // Update the value of the health slider based on the current health of the firefly
            healthSlider.value = AirflyController.health;
        }
    }
}
