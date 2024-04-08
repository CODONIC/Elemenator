using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f; // Adjust this value to control the speed

   
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 joystickInput = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);

        MoveCharacter(joystickInput);
        UpdateAnimation(joystickInput);
    }

    public Vector3 GetFacingDirection()
    {
        // Get the rotation of the player's sprite
        Quaternion rotation = transform.rotation;

        // Calculate the facing direction based on the sprite's rotation
        Vector3 facingDirection = Vector3.up; // Default facing direction is up (0, 1, 0)

        // Determine the angle between the forward direction and each cardinal direction
        float angleUp = Vector3.Angle(transform.up, Vector3.up);
        float angleRight = Vector3.Angle(transform.right, Vector3.up);
        float angleDown = Vector3.Angle(-transform.up, Vector3.up);
        float angleLeft = Vector3.Angle(-transform.right, Vector3.up);

        // Find the direction closest to the current rotation
        float minAngle = Mathf.Min(angleUp, angleRight, angleDown, angleLeft);
        if (minAngle == angleUp)
        {
            // Facing up
            facingDirection = Vector3.up;
        }
        else if (minAngle == angleRight)
        {
            // Facing right
            facingDirection = Vector3.right;
        }
        else if (minAngle == angleDown)
        {
            // Facing down
            facingDirection = Vector3.down;
        }
        else if (minAngle == angleLeft)
        {
            // Facing left
            facingDirection = Vector3.left;
        }
        return facingDirection;
    }

    public void MoveCharacter(Vector3 input)
    {
        Vector3 movement = input.normalized * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
    
    void UpdateAnimation(Vector3 input)
    {
        if (input != Vector3.zero)
        {
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

   
}