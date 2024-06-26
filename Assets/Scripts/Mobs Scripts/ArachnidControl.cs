using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArachnidControl : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float health = 100f;

    public float flashDuration = 1f; // Duration of the flash effect
    private SpriteRenderer spriteRenderer;
    public float showHealthContainerDistance;

    public GameObject arachnidHealthContainer;

    public float attackCooldown;
    private float nextAttackTime;

    public int damage = 10;
    public Transform player;

    // Animator component
    private Animator animator;

    //LootTable
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    private bool isChasing = true; // Flag to track whether the enemy is chasing the player
    private bool isAttacking = false; // Flag to track whether the enemy is attacking

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        nextAttackTime = Time.time;

        if (arachnidHealthContainer != null)
        {
            arachnidHealthContainer.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) return;

        // Check if the player is in range to show the health container
        if (Vector2.Distance(transform.position, player.position) <= showHealthContainerDistance)
        {
            if (arachnidHealthContainer != null && !arachnidHealthContainer.activeSelf)
            {
                arachnidHealthContainer.SetActive(true);
                isChasing = true; // Resume chasing when health container is activated
            }
        }
        else
        {
            if (arachnidHealthContainer != null && arachnidHealthContainer.activeSelf)
            {
                arachnidHealthContainer.SetActive(false);
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
        isAttacking = false;
        animator.SetBool("IsChasing", true);
        animator.SetBool("IsAttacking", false);

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", true);

        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
            playerComponent.TakeDamage(damage);
        }
    }

    private void FlipSprite()
    {
        // Check if the player is to the left or right of the arachnid
        if (player.position.x < transform.position.x)
        {
            // Player is to the left, so face left
            spriteRenderer.flipX = false;
        }
        else
        {
            // Player is to the right, so face right
            spriteRenderer.flipX = true;
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
            StartCoroutine(FlashSprite());
        }
    }

    private IEnumerator FlashSprite()
    {
        Color currentColor = spriteRenderer.color;
        Color flashColor = new Color(1f - currentColor.r, 1f - currentColor.g, 1f - currentColor.b);

        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = currentColor;
    }

    private bool isDying = false;

    private void Die()
    {
        if (isDying) return;

        isDying = true;
        animator.SetBool("IsDead", true);

        foreach (LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
            }
        }

        StartCoroutine(DestroyAfterAnimation());
    }

    private void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            Instantiate(loot, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().color = Color.white;
        }
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
