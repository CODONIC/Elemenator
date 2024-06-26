using UnityEngine;
using UnityEngine.UI;

public class FireFlyHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider

    private FireFlyControl fireflyController; // Reference to the FireflyController script

    void Start()
    {
        // Find and store a reference to the FireflyController script attached to the firefly
        fireflyController = GetComponent<FireFlyControl>();

        // Set the maximum value of the health slider to the initial health of the firefly
        healthSlider.maxValue = fireflyController.health;
        // Set the current value of the health slider to the initial health of the firefly
        healthSlider.value = fireflyController.health;
    }

    void Update()
    {
        // Check if fireflyController is not null before using it
        if (fireflyController != null)
        {
            // Update the value of the health slider based on the current health of the firefly
            healthSlider.value = fireflyController.health;
        }
    }

}
