using UnityEngine;

public class AttackFunction : MonoBehaviour
{
    public WeaponParent weaponParent; // Reference to the WeaponParent script

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        // Check if the weaponParent reference is set
        if (weaponParent == null)
        {
            Debug.LogError("WeaponParent reference is not set. Make sure to assign it in the Inspector.");
            return;
        }

        // Call the Attack method from the WeaponParent script
        weaponParent.Attack();

        // If needed, you can add additional attack logic here
        Debug.Log("Basic attack executed!");
    }
}
