using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WeaponParent : MonoBehaviour
{
    public Animator leftGauntletAnimator;
    public Animator rightGauntletAnimator;
    public float delay = 0.3f;
    private bool attackBlocked;
    private bool isLeftGauntletAttacking;

    // Reference to the joystick
    public Joystick joystick;

    private Vector3 initialScaleLeft; // Store the initial scale of the left gauntlet
    private Vector3 initialScaleRight; // Store the initial scale of the right gauntlet

    public float rotationSpeed = 5f;
    public float autoTargetRange = 10f; // Range within which enemies will be targeted automatically

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
        FindJoystick();
        // Ensure both gauntlets start with their initial rotation
        leftGauntletAnimator.transform.rotation = Quaternion.identity;
        rightGauntletAnimator.transform.rotation = Quaternion.identity;

        // Store the initial scale of the gauntlets
        initialScaleLeft = leftGauntletAnimator.transform.localScale;
        initialScaleRight = rightGauntletAnimator.transform.localScale;

        // Start with left gauntlet attacking
        isLeftGauntletAttacking = true;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the scene loaded event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindJoystick(); // Call this method to find the joystick whenever a new scene is loaded
    }

    private void FindJoystick()
    {
        // Find and assign the Joystick reference if it's not set
        if (joystick == null)
        {
            joystick = FindObjectOfType<FixedJoystick>();
            if (joystick == null)
            {
                Debug.LogError("Joystick reference is not set and could not be found in the scene.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Joystick reference is valid before using it
        if (joystick != null)
        {
            // Perform operations dependent on the Joystick
            HandleWeaponRotation();
        }
        // Call the method to handle weapon rotation based on joystick input
        HandleWeaponRotation();

        // Auto-target enemies
        AutoTargetEnemies();
    }

    public void Attack()
    {
        if (attackBlocked)
            return;

        if (isLeftGauntletAttacking)
            leftGauntletAnimator.SetTrigger("Attack");
        else
            rightGauntletAnimator.SetTrigger("Attack");

        // Toggle between left and right gauntlet attack
        isLeftGauntletAttacking = !isLeftGauntletAttacking;

        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void HandleWeaponRotation()
    {
        // Ensure that the joystick reference is not null
        if (joystick == null)
        {
            Debug.LogError("Joystick reference is not set. Make sure to assign it in the Inspector.");
            return;
        }

        // Get the direction of joystick input
        Vector3 direction = new Vector3(joystick.Horizontal, joystick.Vertical, 0f).normalized;

        // Ensure that the direction vector is not zero
        if (direction.magnitude > 0.1f)
        {
            // Calculate the angle between the weapon's forward direction and the joystick input direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the rotation of the left gauntlet
            if (direction.x < 0)
            {
                // If moving left, flip the rotation by adding 180 degrees
                leftGauntletAnimator.transform.localRotation = Quaternion.Euler(0f, 0f, angle + 180f);
                leftGauntletAnimator.transform.localScale = new Vector3(-initialScaleLeft.x, initialScaleLeft.y, initialScaleLeft.z); // Flip horizontally if moving left
            }
            else
            {
                leftGauntletAnimator.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
                leftGauntletAnimator.transform.localScale = initialScaleLeft; // Restore the initial scale
            }

            // Set the rotation of the right gauntlet
            if (direction.x < 0)
            {
                // If moving left, flip the rotation by adding 180 degrees
                rightGauntletAnimator.transform.localRotation = Quaternion.Euler(0f, 0f, angle + 180f);
                rightGauntletAnimator.transform.localScale = new Vector3(-initialScaleRight.x, initialScaleRight.y, initialScaleRight.z); // Flip horizontally if moving left
            }
            else
            {
                rightGauntletAnimator.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
                rightGauntletAnimator.transform.localScale = initialScaleRight; // Restore the initial scale
            }
        }
    }

    private void AutoTargetEnemies()
    {
        // Find all GameObjects with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check if there are any enemies
        if (enemies.Length == 0)
            return;

        // Find the nearest enemy within auto-target range
        GameObject nearestEnemy = null;
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

        // If a nearest enemy is found, calculate direction and angle towards it
        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the rotation of the gauntlets towards the enemy
            if (direction.x < 0)
            {
                // If moving left, flip the rotation by adding 180 degrees
                leftGauntletAnimator.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
                leftGauntletAnimator.transform.localScale = new Vector3(-initialScaleLeft.x, initialScaleLeft.y, initialScaleLeft.z); // Flip horizontally if moving left

                rightGauntletAnimator.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
                rightGauntletAnimator.transform.localScale = new Vector3(-initialScaleRight.x, initialScaleRight.y, initialScaleRight.z); // Flip horizontally if moving left
            }
            else
            {
                leftGauntletAnimator.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                leftGauntletAnimator.transform.localScale = initialScaleLeft; // Restore the initial scale

                rightGauntletAnimator.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                rightGauntletAnimator.transform.localScale = initialScaleRight; // Restore the initial scale
            }
        }
        else
        {
            // No enemy found, revert to using joystick input for rotation
            HandleWeaponRotation();
        }
    }




}
