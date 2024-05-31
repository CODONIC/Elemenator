using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GolemHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider

    private GolemnControl golemController; // Reference to the GolemControl script

    void Start()
    {
        // Find and store a reference to the GolemControl script in the parent or sibling object
        golemController = GetComponentInParent<GolemnControl>();
        if (golemController == null)
        {
            golemController = GetComponentInChildren<GolemnControl>();
        }

        // Set the maximum value of the health slider to the initial health of the Golem
        if (golemController != null)
        {
            healthSlider.maxValue = golemController.health;
            // Set the current value of the health slider to the initial health of the Golem
            healthSlider.value = golemController.health;
        }
    }

    void Update()
    {
        // Check if golemController is not null before using it
        if (golemController != null)
        {
            // Update the value of the health slider based on the current health of the Golem
            healthSlider.value = golemController.health;
        }
    }
}
