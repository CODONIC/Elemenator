using UnityEngine;
using UnityEngine.UI;

public class SlimeHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider

    private SlimeController slimeController; // Reference to the SlimeController script

    void Start()
    {
        // Find and store a reference to the SlimeController script attached to the slime
        slimeController = GetComponent<SlimeController>();

        // Set the maximum value of the health slider to the initial health of the slime
        healthSlider.maxValue = slimeController.health;
        // Set the current value of the health slider to the initial health of the slime
        healthSlider.value = slimeController.health;
    }

    void Update()
    {
        // Update the value of the health slider based on the current health of the slime
        healthSlider.value = slimeController.health;
    }
}
