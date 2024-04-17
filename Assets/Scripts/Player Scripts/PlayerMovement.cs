using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f; // Adjust this value to control the speed

   
    private Animator animator;

    private void Awake()
    {
        // Initialize references on scene load
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure references are initialized after scene load
        FindJoystick();
        FindAnimator();
    }

    void Start()
    {
        // Find and assign the Animator component
        FindAnimator();
    }

    void Update()
    {
        // Ensure joystick reference is not null before using it
        if (joystick != null)
        {
            Vector3 joystickInput = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);

            MoveCharacter(joystickInput);
            UpdateAnimation(joystickInput);
        }
        else
        {
            Debug.LogWarning("Joystick reference is null. Ensure it's properly assigned in the Inspector.");
        }
    }
    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void FindJoystick()
    {
        // Find and assign the FixedJoystick component
        joystick = FindObjectOfType<FixedJoystick>();
        if (joystick == null)
        {
            Debug.LogError("FixedJoystick reference not found in the scene!");
        }
    }

    private void FindAnimator()
    {
        // Find and assign the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player GameObject!");
        }
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