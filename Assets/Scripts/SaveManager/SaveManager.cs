using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Method to save the player's health
    public void SavePlayerHealth(int health)
    {
        PlayerPrefs.SetInt("PlayerHealth", health);
        PlayerPrefs.Save();
        Debug.Log("Player health saved: " + health);
    }

    // Method to save the player's position
    public void SavePlayerPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerPositionX", position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", position.z);
        PlayerPrefs.Save();
        Debug.Log("Player position saved: " + position);
    }
}
