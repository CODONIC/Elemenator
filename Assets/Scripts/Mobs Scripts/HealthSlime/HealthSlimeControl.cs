using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSlimeControl : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float health = 50f;

    public float showHealthContainerDistance;
    public GameObject healthSlimeHealthContainer;

    public float attackCooldown;
    private float nextAttackTime;

    public int damage = 5;
    public int healthToGive = 20; // Amount of health to give to the player when killed
    public Transform player;

    // Animator component
    private Animator animator;

    private bool isChasing = true; // Flag to track whether the enemy is chasing the player
    private bool isAttacking = false; // Flag to track whether the enemy is attacking

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        nextAttackTime = Time.time;

        if (healthSlimeHealthContainer != null)
        {
            healthSlimeHealthContainer.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) return;

        // Check if the player is in range to show the health container
        if (Vector2.Distance(transform.position, player.position) <= showHealthContainerDistance)
        {
            if (healthSlimeHealthContainer != null && !healthSlimeHealthContainer.activeSelf)
            {
                healthSlimeHealthContainer.SetActive(true);
                isChasing = true; // Resume chasing when health container is activated
            }
        }
        else
        {
            if (healthSlimeHealthContainer != null && healthSlimeHealthContainer.activeSelf)
            {
                healthSlimeHealthContainer.SetActive(false);
                isChasing = false; // Stop chasing when health container is deactivated
            }
        }

        if (!isChasing) return;

        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            ChasePlayer();
        }
        else
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        // Flip the sprite based on the player's position
        FlipSprite();
    }

    private void ChasePlayer()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Damaged")) return;

        isAttacking = false;
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsIdle", false);

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Damaged")) return;

        isAttacking = true;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsIdle", false);

        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
            playerComponent.TakeDamage(damage);
        }
    }

    private void FlipSprite()
    {
        // Check if the player is to the left or right of the slime
        if (player.position.x > transform.position.x)
        {
            // Player is to the left, so face left
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            // Player is to the right, so face right
            GetComponent<SpriteRenderer>().flipX = true;
        }
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
            // Play the damage animation
            animator.SetTrigger("IsDamaged");
            StartCoroutine(ResetDamaged());
        }
    }

    private IEnumerator ResetDamaged()
    {
        yield return new WaitForSeconds(0.5f); // Adjust to the duration of your damaged animation
        animator.ResetTrigger("IsDamaged");
        if (!isAttacking && isChasing)
        {
            animator.SetBool("IsWalking", true);
        }
        else if (!isChasing)
        {
            animator.SetBool("IsIdle", true);
        }
    }

    private bool isDying = false;

    private void Die()
    {
        if (isDying) return;

        isDying = true;
        animator.SetBool("IsDead", true);

        // Give health to the player
        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
           // playerComponent.GiveHealth(healthToGive);
        }

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1f);

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
