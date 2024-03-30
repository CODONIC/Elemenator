using UnityEngine;
using System.Collections;

public class WeaponParent : MonoBehaviour
{
    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;

    // Reference to the joystick
    public Joystick joystick;

    // Update is called once per frame
    void Update()
    {
        // Call the method to handle weapon rotation based on joystick input
        HandleWeaponRotation();
    }

    public void Attack()
    {
        if (attackBlocked)
            return;
        animator.SetTrigger("Attack");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    public float rotationSpeed = 5f;

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

            // Calculate the desired rotation based on the angle
            Quaternion desiredRotation;

            // Check if the player is moving left
            if (direction.x < 0)
            {
                // If moving left, flip the rotation by adding 180 degrees
                desiredRotation = Quaternion.Euler(0f, 0f, angle + 180f);

                // Flip the weapon horizontally
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                // Otherwise, use the normal rotation
                desiredRotation = Quaternion.Euler(0f, 0f, angle);

                // Ensure the weapon is facing right
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

            // Smoothly interpolate the weapon's rotation towards the desired rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);

            if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
            {
                // Handle sorting order based on rotation angle
                Debug.Log("Sorting Order Decremented");
            }
            else
            {
                // Handle sorting order based on rotation angle
                Debug.Log("Sorting Order Incremented");
            }
        }
    }


}
