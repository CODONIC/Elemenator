using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class dodge : MonoBehaviour
{
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public float dodgeSpeed = 10f; // Increase this value to make the dodge faster
    public float dodgeDuration = 0.5f; // Adjust this value to control the duration of the dodge
    public float dodgeCooldown = 0.5f; // Cooldown duration between dodges

    private bool isDodging = false;
    private bool isOnCooldown = false;

    void Start()
    {
        // Add a listener to the UI button click event
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        Debug.Log("Dodge button clicked");
        // Call PerformDodge only if not already dodging and not on cooldown
        if (!isDodging && !isOnCooldown)
        {
            Debug.Log("Performing dodge");
            PerformDodge();
            StartCoroutine(StartCooldown());
        }
    }

    IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        Debug.Log("Cooldown started");
        yield return new WaitForSeconds(dodgeCooldown);
        isOnCooldown = false;
        Debug.Log("Cooldown ended");

        // Enable dodge button again after cooldown ends
        GetComponent<Button>().interactable = true;
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
        // Return the direction the player is facing
        Vector3 facingDirection = playerMovement.GetFacingDirection();
        Debug.Log("Dodge direction: " + facingDirection);
        return facingDirection;
    }

    IEnumerator Dodge(Vector3 direction)
    {
        isDodging = true;
        Debug.Log("Dodge started"); 

        // Initial position
        Vector3 startPosition = playerMovement.transform.position;
        // Target position based on dodge direction and speed
        Vector3 targetPosition = startPosition + direction.normalized * dodgeSpeed;

        float startTime = Time.time;
        while (Time.time - startTime < dodgeDuration)
        {
            // Smoothly interpolate the player's position to the target position
            playerMovement.transform.position = Vector3.Lerp(startPosition, targetPosition, (Time.time - startTime) / dodgeDuration);
            yield return null;
        }

        isDodging = false;
        Debug.Log("Dodge ended");
    }
}
