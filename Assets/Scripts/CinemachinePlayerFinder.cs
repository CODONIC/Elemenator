using UnityEngine;
using Cinemachine;

public class CinemachinePlayerFinder : MonoBehaviour
{
    // Array to store references to all Cinemachine Virtual Cameras
    public CinemachineVirtualCamera[] cinemachineCameras;

    void Start()
    {
        // Find the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player object not found.");
            return;
        }

        // Loop through each Cinemachine Virtual Camera and assign the player object as the follow target
        foreach (CinemachineVirtualCamera cinemachineCamera in cinemachineCameras)
        {
            if (cinemachineCamera != null)
            {
                cinemachineCamera.Follow = player.transform;
            }
        }
    }
}
