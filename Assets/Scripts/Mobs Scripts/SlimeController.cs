using UnityEngine;
using System.Collections;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float health = 50f;
    public int damage = 10;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;
    public float delayAfterCollision = 1f;
    public float dropChancePercentage = 75f;

    public GameObject slimeHealthContainer;
    public GameObject itemPrefab1;
    public GameObject itemPrefab2;// Reference to the prefab of the item to drop

    private Transform target;
    public bool canMove;
    private Collider2D detectionCollider;

    private bool isHitAnimationPlaying = false;

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

                if (distanceToPlayer <= detectionRange)
                {
                    canMove = true;

                    if (slimeHealthContainer != null)
                    {
                        slimeHealthContainer.SetActive(true);
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

                    transform.Translate(direction * moveSpeed * Time.deltaTime);

                    TriggerJumpAnimation(true);

                    if (distanceToPlayer <= attackRange)
                    {
                        // Implement attack logic here
                    }
                }
                else
                {
                    canMove = false;
                    TriggerJumpAnimation(false);

                    if (slimeHealthContainer != null)
                    {
                        slimeHealthContainer.SetActive(false);
                    }
                }
            }
            else
            {
                delayTimer -= Time.deltaTime;
                if (delayTimer <= 0)
                {
                    isDelayingMovement = false;
                    canMove = true;
                }
            }
        }
    }

    void FlipSprite(bool faceLeft)
    {
        // Get the current local scale
        Vector3 scale = transform.localScale;

        // Set the x-scale based on whether the slime should face left
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




    void TriggerHitAnimation(bool isHit)
    {
        Animator Hitanimator = GetComponent<Animator>();
        if (Hitanimator != null)
        {
            Hitanimator.SetBool("IsHit", isHit);
        }
    }

    void TriggerJumpAnimation(bool isJumping)
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsJumping", isJumping);
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
            TriggerHitAnimation(true);
            isHitAnimationPlaying = true;
            canMove = false;
            StartDelayBeforeChase();
            StartCoroutine(ResetHitAnimation());
        }
    }

    IEnumerator ResetHitAnimation()
    {
        yield return new WaitForSeconds(0.5f);
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
        yield return new WaitForSeconds(delayAfterCollision);
        canMove = true;
    }

    public void TakeDamage(float damage)
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
        delayTimer = delayAfterCollision;
    }
    private bool isDying = false;
    void Die()
    {
        // Check if the slime is already dying
        if (isDying)
        {
            return;
        }

        // Set the flag to true to indicate that the slime is dying
        isDying = true;

        // Stop movement immediately
        canMove = false;

        // Trigger death animation if available
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsDead", true); // Assuming you have a bool parameter named "IsDead" in your animator controller
        }

        // Randomly determine if the item prefabs should drop
        bool shouldDropItem1 = Random.Range(0f, 100f) < dropChancePercentage;
        bool shouldDropItem2 = Random.Range(0f, 100f) < dropChancePercentage;

        // Instantiate the item prefabs at the slime's position if they should drop
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


    public void ApplyKnockback(Vector2 direction, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        Destroy(gameObject);
    }
}
