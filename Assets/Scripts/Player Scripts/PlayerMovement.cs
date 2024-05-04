using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f; // Adjust this value to control the speed
    public string enemyTag = "Enemy"; // Tag of the enemy game objects
    public float autoTargetRange = 5f; // Range within which enemies will be automatically targeted

    private Animator animator;
    private GameObject nearestEnemy;
    private Vector3 lastJoystickInput; // Store the last joystick input

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
        // Check if the Joystick reference is valid before using it
        if (joystick != null)
        {
            // Perform operations dependent on the Joystick
            HandlePlayerRotation();
            MoveCharacter(lastJoystickInput); // Move the player with the last joystick input
            UpdateAnimation(lastJoystickInput);

            // Auto-target enemies
            AutoTargetEnemies();
        }
        else
        {
            Debug.LogWarning("Joystick reference is null. Ensure it's properly assigned in the Inspector.");
        }
    }

    private void HandlePlayerRotation()
    {
<<<<<<< HEAD
        // Get the facing direction based on the sprite's transform.up direction
        return transform.up;
=======
        // Ensure that the joystick reference is not null
        if (joystick == null)
        {
            Debug.LogError("Joystick reference is not set. Make sure to assign it in the Inspector.");
            return;
        }

        // Get the direction of joystick input
        Vector3 joystickInput = new Vector3(joystick.Horizontal, joystick.Vertical, 0f).normalized;
        lastJoystickInput = joystickInput; // Store the last joystick input
    }

    private void AutoTargetEnemies()
    {
        // Find all GameObjects with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // Find the nearest enemy within auto-target range
        nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= autoTargetRange && distance < nearestDistance)
            {
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }

        // If a nearest enemy is found, calculate direction towards it and update facing direction
        if (nearestEnemy != null)
        {
            Vector3 directionToEnemy = (nearestEnemy.transform.position - transform.position).normalized;

            // Determine the animation direction based on the direction vector to the enemy
            if (Mathf.Abs(directionToEnemy.x) > Mathf.Abs(directionToEnemy.y))
            {
                if (directionToEnemy.x > 0)
                {
                    // Facing right
                    animator.SetFloat("moveX", 1f);
                    animator.SetFloat("moveY", 0f);
                }
                else
                {
                    // Facing left
                    animator.SetFloat("moveX", -1f);
                    animator.SetFloat("moveY", 0f);
                }
            }
            else
            {
                if (directionToEnemy.y > 0)
                {
                    // Facing up
                    animator.SetFloat("moveX", 0f);
                    animator.SetFloat("moveY", 1f);
                }
                else
                {
                    // Facing down
                    animator.SetFloat("moveX", 0f);
                    animator.SetFloat("moveY", -1f);
                }
            }
        }
        else
        {
            // No enemy found, revert to using joystick input for rotation
            HandlePlayerRotation();
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
>>>>>>> 25a656219834cb8541e6bee1f35c6c925884548f
    }

    public void MoveCharacter(Vector3 input)
    {
        // Move the character only if there's input and respecting collisions
        if (input != Vector3.zero)
        {
            Vector3 movement = input.normalized * moveSpeed * Time.deltaTime;
            transform.position += movement;
        }
    }

    void UpdateAnimation(Vector3 input)
    {
        // Update the animation based on the input
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
<<<<<<< HEAD
=======

    public Vector3 GetFacingDirection()
    {
        // Calculate the facing direction towards the nearest enemy
        if (nearestEnemy != null)
        {
            Debug.Log("There are enemies, dash using enemy");
            Vector3 direction = (transform.position - nearestEnemy.transform.position).normalized; // Reverse the direction
            return direction;
        }
        else 
        {
            Debug.Log("No enemies, dash using joystick");
            // If no nearest enemy is found, use the last joystick input direction
            Vector3 joystickInput = lastJoystickInput.normalized;
            return joystickInput;
        }
    }

>>>>>>> 25a656219834cb8541e6bee1f35c6c925884548f
}
