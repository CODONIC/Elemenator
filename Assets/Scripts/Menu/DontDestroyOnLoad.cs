using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        // Prevent the GameObject this script is attached to from being destroyed when loading a new scene
        DontDestroyOnLoad(gameObject);
    }
}
