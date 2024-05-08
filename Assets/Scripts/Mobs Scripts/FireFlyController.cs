using UnityEngine;
using System.Collections;

public class FireFlyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float health = 50f;
    public int damage = 10;
    public float attackRange = 5f;
    public float detectionRange = 10f;
    public float delayAfterCollision = 1f;
    public float dropChancePercentage = 75f;
    public float flashDuration = 1.5f; // Duration of the flash effect
    public Color flashColor = Color.white; // Color to flash the sprite
    public GameObject fireballPrefab;

    public GameObject fireflyHealthContainer;
    public GameObject itemPrefab1;
    public GameObject itemPrefab2; // Reference to the prefab of the item to drop

    private Transform target;
    public bool canMove;
    private Collider2D detectionCollider;

    private bool isHitAnimationPlaying = false;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

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
                detectionCollider.isTrigger = true;
            }
        }
        else
        {
            Debug.LogError("Detection transform not found!");
        }

        if (fireflyHealthContainer != null)
        {
            fireflyHealthContainer.SetActive(false);
        }

        // Get the SpriteRenderer component attached to the enemy object
        spriteRenderer = GetComponent<SpriteRenderer>();

   
    }

    private float delayTimer = 0f;
    private bool isDelayingMovement = false;

    public float fireballCooldown = 5f; // Cooldown between fireball shots
    private float fireballCooldownTimer = 5.5f; // Timer to track fireball cooldown

    void Update()
    {
        if (health > 0 && target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Show the health container when the player is nearby
                if (fireflyHealthContainer != null)
                {
                    fireflyHealthContainer.SetActive(true);
                }

                Vector2 direction = (target.position - transform.position).normalized;

                // Flip sprite if player is on the left side
                if (direction.x < 0)
                {
                    FlipSprite(true);
                }
                else
                {
                    FlipSprite(false);
                }

                if (distanceToPlayer <= attackRange)
                {
                    // Stop movement when within attack range
                    canMove = false;

                    // Implement attack logic here
                    Attack();
                }
                else
                {
                    // Move towards the player if not in attack range
                    canMove = true;
                    transform.Translate(direction * moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                // Hide the health container when the player is not nearby
                if (fireflyHealthContainer != null)
                {
                    fireflyHealthContainer.SetActive(false);
                }

                canMove = false;
            }
        }
        // Decrement fireball cooldown timer
        fireballCooldownTimer -= Time.deltaTime;
    }





    void Attack()
    {
        // Check if fireball cooldown is over and enemy is within range
        if (fireballCooldownTimer <= 0f)
        {
            // Shoot fireballs towards the player
            ShootFireball();

            // Reset fireball cooldown timer
            fireballCooldownTimer = fireballCooldown;
        }
    }
    public float speed = 3f; // Speed of the fireball

    void ShootFireball()
    {
        if (fireballPrefab == null)
        {
            Debug.LogError("Fireball prefab is not assigned!");
            return;
        }

        // Calculate direction towards the player
        Vector2 direction = (target.position - transform.position).normalized;

        // Log the direction vector
        Debug.Log("Direction towards player: " + direction);

        // Calculate spawn position with an offset from the enemy's position
        Vector2 spawnPosition = (Vector2)transform.position + direction * 2f; // Adjust the offset as needed

        // Instantiate the fireball prefab at the spawn position
        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);

        if (fireball == null)
        {
            Debug.LogError("Failed to instantiate fireball prefab!");
            return;
        }

        // Get the Rigidbody2D component of the fireball
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on fireball prefab!");
            return;
        }

        // Set the velocity of the fireball to move towards the player
        rb.velocity = direction * moveSpeed;

        Debug.Log("Fireball spawned and launched!");
    }







    void FlipSprite(bool faceLeft)
    {
        // Get the current local scale
        Vector3 scale = transform.localScale;

        // Set the x-scale based on whether the firefly should face left
        if (faceLeft)
        {
            scale.x = Mathf.Abs(scale.x) * -1f;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        // Apply the new x-scale to the SpriteRenderer
        GetComponent<SpriteRenderer>().flipX = (scale.x < 0);
    }
    public void ApplyKnockback(Vector2 direction, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player playerController = collision.collider.GetComponentInParent<Player>();
            if (playerController != null)
            {
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
            
            isHitAnimationPlaying = true;
            canMove = false;
            StartDelayBeforeChase();
            StartCoroutine(ResetHitAnimation());
        }
    }

  

    IEnumerator ResetHitAnimation()
    {
        yield return new WaitForSeconds(0.5f);
       
        isHitAnimationPlaying = false;
        ResumeMovement();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            Debug.Log("Player weapon exit detected. Stopping hit animation.");
            
            isHitAnimationPlaying = false;
            ResumeMovement();
        }
    }

    void ResumeMovement()
    {
        canMove = true;
    }

    void StartDelayBeforeChase()
    {
        isDelayingMovement = true;
        delayTimer = delayAfterCollision;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Start the flash effect coroutine
            StartCoroutine(FlashSprite());
        }
    }

    IEnumerator FlashSprite()
    {
        // Get the current color of the sprite
        Color currentColor = spriteRenderer.color;

        // Calculate the contrast color for flashing
        Color flashColor = new Color(1f - currentColor.r, 1f - currentColor.g, 1f - currentColor.b);

        // Set the sprite color to the flash color
        spriteRenderer.color = flashColor;

        // Wait for the specified duration
        yield return new WaitForSeconds(flashDuration);

        // Revert the sprite color back to its original color
        spriteRenderer.color = currentColor;
    }

    private bool isDying = false; // Flag to track whether the enemy is already dying

    void Die()
    {
        // Check if the enemy is already dying
        if (!isDying && health <= 0)
        {
            // Set the flag to true to indicate that the enemy is dying
            isDying = true;

            // Trigger death animation if available
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                // Set the "IsDead" parameter to true in the Animator Controller
                animator.SetBool("IsDead", true);

                // Adjust the animation speed
                animator.speed = 0.5f; // Adjust this value as needed to slow down the animation
            }

            // Randomly determine if the item prefabs should drop
            bool shouldDropItem1 = Random.Range(0f, 100f) < dropChancePercentage;
            bool shouldDropItem2 = Random.Range(0f, 100f) < dropChancePercentage;

            // Instantiate the item prefabs at the enemy's position if they should drop
            if (shouldDropItem1 && itemPrefab1 != null)
            {
                Instantiate(itemPrefab1, transform.position, Quaternion.identity);
            }
            if (shouldDropItem2 && itemPrefab2 != null)
            {
                Instantiate(itemPrefab2, transform.position, Quaternion.identity);
            }

            // Destroy the game object after the animation duration
            StartCoroutine(DestroyAfterAnimation());
        }
    }





    IEnumerator DestroyAfterAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            // Wait for the animation to start playing
            yield return new WaitForSeconds(1f); // Adjust the delay as needed

            // Loop until the animation has finished playing
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
        }

        // Animation has finished playing, destroy the game object
        Destroy(gameObject);
    }




}
