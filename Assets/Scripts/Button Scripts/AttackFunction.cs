using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackFunction : MonoBehaviour
{
    public WeaponParent weaponParent; // Reference to the WeaponParent script
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip[] attackSounds; // Array to hold multiple attack sound effects

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
        FindWeaponParent(); // Call this method to find the WeaponParent script in the current scene
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the scene loaded event
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindWeaponParent(); // Call this method to find the WeaponParent script whenever a new scene is loaded
    }

    void FindWeaponParent()
    {
        weaponParent = FindObjectOfType<WeaponParent>();
        if (weaponParent == null)
        {
            Debug.LogError("WeaponParent reference not found in the scene!");
            enabled = false; // Disable the script if WeaponParent reference is missing
        }
    }

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

        // Play a random attack sound
        if (attackSounds.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, attackSounds.Length);
            audioSource.PlayOneShot(attackSounds[randomIndex]);
        }
        else
        {
            Debug.LogError("Attack sounds array is empty or AudioSource is not assigned.");
        }

        // If needed, you can add additional attack logic here
        Debug.Log("Basic attack executed!");
    }
}
    