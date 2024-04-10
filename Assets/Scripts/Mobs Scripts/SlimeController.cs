using UnityEngine;
using System.Collections;

public class SlimeController : MonoBehaviour
{
    public GameObject elementPrefab; // Reference to the element prefab to drop
    public float dropChance = 0.5f; // Probability of dropping an element (0 to 1)

    public float moveSpeed = 3f;
    public int health = 50;
    public int damage = 10;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;
    public float delayAfterCollision = 1f; // Delay in seconds after colliding with player

    public GameObject slimeHealthContainer; // Reference to the health bar GameObject
    private Transform target;
    public bool canMove; // Flag to control whether the slime can move
    private Collider2D detectionCollider; // Reference to the detection range collider

    private bool isHitAnimationPlaying = false; // Flag to track if hit animation is playing
    private bool isJumpAnimationPaused = false; // Flag to track if jump animation is paused

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

    void Update()
    {
        if (health > 0 && target != null)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitBox"))
        {
            Debug.Log("Player hit detected. Stopping movement.");
            canMove = false;
            TriggerJumpAnimation(false);

            // Start delay coroutine before resuming movement
            StartCoroutine(DelayBeforeChase());

            // Damage the player only if the slime is actively chasing
            if (canMove)
            {
                Player playerController = other.GetComponentInParent<Player>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damage);
                }
            }
        }
        else if (other.CompareTag("PlayerWeapon") && !isHitAnimationPlaying)
        {
            Debug.Log("Player weapon hit detected. Triggering hit animation.");
            TriggerHitAnimation(true);
            isHitAnimationPlaying = true;
            // Reset hit animation after a delay
            StartCoroutine(ResetHitAnimation());
        }
        else if (other.CompareTag("PlayerHitBox") && other.GetType() == typeof(CapsuleCollider2D))
        {
            // Damage the player only if it collides with the capsule collider of the slime
            
                Player playerController = other.GetComponent<Player>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damage);
                }
            
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

    void PauseJumpAnimation()
    {
        isJumpAnimationPaused = true;
        TriggerJumpAnimation(false);
    }

    void ResumeJumpAnimation()
    {
        isJumpAnimationPaused = false;
        TriggerJumpAnimation(true);
    }

    IEnumerator DelayBeforeChase()
    {
        yield return new WaitForSeconds(delayAfterCollision); // Wait for delay
        canMove = true; // Enable movement again
    }

    public void TakeDamage(int damage)
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
