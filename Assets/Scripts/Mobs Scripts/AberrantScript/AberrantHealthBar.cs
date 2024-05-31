using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AberrantHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider

    private AberrantControl aberrantController; // Reference to the AberrantControl script

    void Start()
    {
        // Find and store a reference to the AberrantControl script in the parent or sibling object
        aberrantController = GetComponentInParent<AberrantControl>();
        if (aberrantController == null)
        {
            aberrantController = GetComponentInChildren<AberrantControl>();
        }

        // Set the maximum value of the health slider to the initial health of the Aberrant
        if (aberrantController != null)
        {
            healthSlider.maxValue = aberrantController.health;
            // Set the current value of the health slider to the initial health of the Aberrant
            healthSlider.value = aberrantController.health;
        }
    }

    void Update()
    {
        // Check if aberrantController is not null before using it
        if (aberrantController != null)
        {
            // Update the value of the health slider based on the current health of the Aberrant
            healthSlider.value = aberrantController.health;
        }
    }
}
