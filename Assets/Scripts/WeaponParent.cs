using UnityEngine;
using System.Collections;

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

    private void Start()
    {
        // Ensure both gauntlets start with their initial rotation
        leftGauntletAnimator.transform.rotation = Quaternion.identity;
        rightGauntletAnimator.transform.rotation = Quaternion.identity;

        // Store the initial scale of the gauntlets
        initialScaleLeft = leftGauntletAnimator.transform.localScale;
        initialScaleRight = rightGauntletAnimator.transform.localScale;

        // Start with left gauntlet attacking
        isLeftGauntletAttacking = true;
    }

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
}
