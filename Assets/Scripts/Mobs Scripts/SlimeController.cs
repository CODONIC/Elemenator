using UnityEngine;
using System.Collections;

public class SlimeController : MonoBehaviour
{
    public GameObject elementPrefab; // Reference to the element prefab to drop
    public float dropChance = 0.5f; // Probability of dropping an element (0 to 1)

    public float moveSpeed = 3f;
    public float health = 50f; // Changed to float
    public int damage = 10;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;
    public float delayAfterCollision = 1f; // Delay in seconds after colliding with player

    public GameObject slimeHealthContainer; // Reference to the health bar GameObject
    private Transform target;
    public bool canMove; // Flag to control whether the slime can move
    private Collider2D detectionCollider; // Reference to the detection range collider

    private bool isHitAnimationPlaying = false; // Flag to track if hit animation is playing
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Find the detection collider in the child object named "Detection"
        Transform detectionTransform = transform.Find("Detection");
        if (detectionTransform != null)
        {
            detectionCollider = detectionTransform.GetComponent<Collider2D>();
            if (detectionCollider == null)
            {
                Debug.LogError("Detection collider not found!");
            }
            else
            {
                detectionCollider.isTrigger = true; // Ensure the collider is set as a trigger
            }
        }
        else
        {
            Debug.LogError("Detection transform not found!");
        }

        // Disable the health bar initially
        if (slimeHealthContainer != null)
        {
            slimeHealthContainer.SetActive(false);
        }
    }

    private float delayTimer = 0f;
    private bool isDelayingMovement = false;

    void Update()
    {
        if (health > 0 && target != null)
        {
            if (!isDelayingMovement)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, target.position);

                // Check if the player is within detection range
                if (distanceToPlayer <= detectionRange)
                {
                    // Player is within range, allow movement
                    canMove = true;

                    // Enable the health bar
                    if (slimeHealthContainer != null)
                    {
                        slimeHealthContainer.SetActive(true);
                    }

                    // Move towards the player
                    Vector2 direction = (target.position - transform.position).normalized;
                    transform.Translate(direction * moveSpeed * Time.deltaTime);

                    // Trigger jump animation
                    TriggerJumpAnimation(true);

                    // Attack if in range
                    if (distanceToPlayer <= attackRange)
                    {
                        // Implement attack logic here
                    }
                }
                else
                {
                    // Player is out of range, stop moving and stop jumping animation
                    canMove = false;
                    TriggerJumpAnimation(false);

                    // Disable the health bar
                    if (slimeHealthContainer != null)
                    {
                        slimeHealthContainer.SetActive(false);
                    }
                }
            }
            else
            {
                // Slime is delaying movement
                delayTimer -= Time.deltaTime;
                if (delayTimer <= 0)
                {
                    // Delay time is over, resume movement
                    isDelayingMovement = false;
                    canMove = true;
                }
            }
        }
    }


    void TriggerHitAnimation(bool isHit)
    {
        // Trigger hit animation if available
        Animator Hitanimator = GetComponent<Animator>();
        if (Hitanimator != null)
        {
            Hitanimator.SetBool("IsHit", isHit); // Assuming you have a bool parameter named "IsHit" in your animator controller
        }
    }

    void TriggerJumpAnimation(bool isJumping)
    {
        // Trigger jump animation if available
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsJumping", isJumping); // Assuming you have a trigger parameter named "Jump" in your animator controller
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Get the player controller from the collided object
            Player playerController = collision.collider.GetComponentInParent<Player>();

            // Check if the player controller exists
            if (playerController != null)
            {
                // Apply damage to the player
                playerController.TakeDamage(damage);

                canMove = false;
                StartDelayBeforeChase();
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("PlayerWeapon") && !isHitAnimationPlaying)
        {
            Debug.Log("Player weapon hit detected. Triggering hit animation.");
            TriggerHitAnimation(true);
            isHitAnimationPlaying = true;
            // Stop movement and start the delay before chase
            canMove = false;
            StartDelayBeforeChase();

            // Reset hit animation after a delay
            StartCoroutine(ResetHitAnimation());
        }

    }



    IEnumerator ResetHitAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Adjust this delay as needed
        TriggerHitAnimation(false);
        isHitAnimationPlaying = false;
        ResumeJumpAnimation();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            Debug.Log("Player weapon exit detected. Stopping hit animation.");
            TriggerHitAnimation(false);
            isHitAnimationPlaying = false;
            ResumeJumpAnimation();
        }
    }



    void ResumeJumpAnimation()
    {
       
        TriggerJumpAnimation(true);
    }

    IEnumerator DelayBeforeChase()
    {
        yield return new WaitForSeconds(delayAfterCollision); // Wait for delay
        canMove = true; // Enable movement again
    }

    public void TakeDamage(float damage) // Changed parameter type to float
    {
        health -= damage;
        if (health < 0)
        {
            TriggerHitAnimation(true);
        }
        if (health <= 0)
        {
            Die();
        }
    }
    void StartDelayBeforeChase()
    {
        isDelayingMovement = true;
        delayTimer = delayAfterCollision; // Set the delay time to 3 seconds
    }
    void Die()
    {
        // Stop movement immediately
        canMove = false;

        // Trigger death animation if available
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsDead", true); // Assuming you have a bool parameter named "IsDead" in your animator controller
        }

        if (Random.value <= dropChance)
        {
            // Instantiate the element prefab at the enemy's position
            Instantiate(elementPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the game object after the animation duration
        StartCoroutine(DestroyAfterAnimation());
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        // Apply force in the given direction
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Reset velocity to prevent interference
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the duration of the death animation
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        // Destroy the game object
        Destroy(gameObject);
    }
}
