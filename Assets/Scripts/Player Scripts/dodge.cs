 using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class dodge : MonoBehaviour
{
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public float dodgeSpeed = 100f; // Increase this value to make the dodge faster
    public float dodgeDuration = 0.5f; // Adjust this value to control the duration of the dodge

    public bool isDodging = false;

    void Start()
    {
        // Add a listener to the UI button click event
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick() // Change to public
    {
        // Call your new function when the button is clickeds
        PerformDodge();
    }

    void PerformDodge()
    {
        // Check if the player is not already dodging
        if (!isDodging)
        {
            // Determine the dodge direction
            Vector3 dodgeDirection = GetDodgeDirection();
            StartCoroutine(Dodge(dodgeDirection));
        }
    }
    Vector3 GetDodgeDirection()
    {
        // Check if the player is providing joystick input
        Vector3 joystickInput = new Vector3(playerMovement.joystick.Horizontal, playerMovement.joystick.Vertical, 0f);
        if (joystickInput != Vector3.zero)
        {
            // If joystick is providing input, dodge in the direction of the joystick
            return joystickInput.normalized;
        }
        else
        {
            // If joystick is not providing input, move straight based on player's facing direction
            return playerMovement.GetFacingDirection();
        }
    } 


    IEnumerator Dodge(Vector3 direction)
    {
        isDodging = true;

        // Apply dodge mechanics
        Vector3 dodgeVelocity = direction.normalized * dodgeSpeed;
        float startTime = Time.time;
        while (Time.time - startTime < dodgeDuration)
        {
            playerMovement.MoveCharacter(dodgeVelocity * Time.deltaTime);
            yield return null;
        }

        isDodging = false;
    }

    // Method to get the facing direction of the player
    Vector3 GetFacingDirection()
    {
        // Get the rotation of the player's sprite
        Quaternion rotation = playerMovement.transform.rotation;

        // Calculate the facing direction based on the sprite's rotation
        Vector3 facingDirection = Vector3.up; // Default facing direction is up (0, 1, 0)
        if (rotation.eulerAngles.z < 45f || rotation.eulerAngles.z > 315f)
        {
            // Facing up
            facingDirection = Vector3.up;
        }
        else if (rotation.eulerAngles.z > 45f && rotation.eulerAngles.z < 135f)
        {
            // Facing right
            facingDirection = Vector3.right;
        }
        else if (rotation.eulerAngles.z > 135f && rotation.eulerAngles.z < 225f)
        {
            // Facing down
            facingDirection = Vector3.down;
        }
        else if (rotation.eulerAngles.z > 225f && rotation.eulerAngles.z < 315f)
        {
            // Facing left
            facingDirection = Vector3.left;
        }

        return facingDirection;
    }
}