using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f; // Adjust this value to control the speed
    public string enemyTag = "Enemy"; // Tag of the enemy game objects

    private Animator animator;
    private GameObject nearestEnemy;

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

            // Find the nearest enemy and update facing direction
            UpdateFacingDirection();
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
        // Calculate the facing direction towards the nearest enemy
        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            return direction;
        }
        else
        {
            // If no nearest enemy is found, default facing direction is up
            return Vector3.up;
        }
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

    void UpdateFacingDirection()
    {
        // Find all game objects with the specified tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // If there are enemies, find the nearest one
        if (enemies.Length > 0)
        {
            float nearestDistance = Mathf.Infinity;
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            // Get the facing direction towards the nearest enemy
            Vector3 facingDirection = GetFacingDirection();

            // Flip the sprite based on the facing direction
            if (facingDirection.x < 0) // Enemy is on the left
            {
                FlipSprite(true);
            }
            else if (facingDirection.x > 0) // Enemy is on the right
            {
                FlipSprite(false);
            }
        }
    }

    void FlipSprite(bool faceLeft)
    {
        // Get the current local scale
        Vector3 scale = transform.localScale;

        // Flip the sprite horizontally based on the faceLeft parameter
        if (faceLeft)
        {
            scale.x = Mathf.Abs(scale.x) * -1f;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        // Apply the new local scale
        transform.localScale = scale;
    }
}
