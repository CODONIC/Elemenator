using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class dodge : MonoBehaviour
{
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public float dodgeSpeed = 200f; // Increase this value to make the dodge faster
<<<<<<< HEAD
    public float dodgeDuration = 1.0f; // Adjust this value to control the duration of the dodge
    public float dodgeCooldown = 2.5f; // Cooldown duration between dodges



=======
    public float dodgeDuration = 1.5f; // Adjust this value to control the duration of the dodge
    public float dodgeCooldown = 0.5f; // Cooldown duration between dodges

>>>>>>> 25a656219834cb8541e6bee1f35c6c925884548f
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
<<<<<<< HEAD
=======

        // Enable dodge button again after cooldown ends
        GetComponent<Button>().interactable = true;
>>>>>>> 25a656219834cb8541e6bee1f35c6c925884548f
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

        // Apply dodge mechanics
        Vector3 dodgeVelocity = direction.normalized * dodgeSpeed;
        float startTime = Time.time;
        while (Time.time - startTime < dodgeDuration)
        {
            // Move the player by dodgeVelocity each frame
            playerMovement.MoveCharacter(dodgeVelocity * Time.deltaTime);
            yield return null;
        }

        isDodging = false;
        Debug.Log("Dodge ended");
    }
}
