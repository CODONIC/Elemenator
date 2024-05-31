using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArachnidHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider

    private ArachnidControl arachnidController; // Reference to the ArachnidControl script

    void Start()
    {
        // Find and store a reference to the ArachnidControl script in the parent or sibling object
        arachnidController = GetComponentInParent<ArachnidControl>();
        if (arachnidController == null)
        {
            arachnidController = GetComponentInChildren<ArachnidControl>();
        }

        // Set the maximum value of the health slider to the initial health of the arachnid
        if (arachnidController != null)
        {
            healthSlider.maxValue = arachnidController.health;
            // Set the current value of the health slider to the initial health of the arachnid
            healthSlider.value = arachnidController.health;
        }
    }

    void Update()
    {
        // Check if arachnidController is not null before using it
        if (arachnidController != null)
        {
            // Update the value of the health slider based on the current health of the arachnid
            healthSlider.value = arachnidController.health;
        }
    }
}
