using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyControl : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float health = 50f;

    public float flashDuration = 1f; // Duration of the flash effect
    private SpriteRenderer spriteRenderer;
    public float showHealthContainerDistance;

    public GameObject fireflyHealthContainer;

    public float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    public Transform player;

    //LootTable
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    private bool isChasing = true; // Flag to track whether the enemy is chasing the player
    private bool isShooting = true; // Flag to track whether the enemy is shooting projectiles

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        timeBtwShots = startTimeBtwShots;

        if (fireflyHealthContainer != null)
        {
            fireflyHealthContainer.SetActive(false);
        }

        // Get the SpriteRenderer component attached to the enemy object
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is in range to show the health container
        if (Vector2.Distance(transform.position, player.position) <= showHealthContainerDistance)
        {
            // If the container is not already active, enable it
            if (fireflyHealthContainer != null && !fireflyHealthContainer.activeSelf)
            {
                fireflyHealthContainer.SetActive(true);
                isChasing = true; // Resume chasing when health container is activated
            }
        }
        else
        {
            // If the container is active and the player is out of range, disable it
            if (fireflyHealthContainer != null && fireflyHealthContainer.activeSelf)
            {
                fireflyHealthContainer.SetActive(false);
                isChasing = false; // Stop chasing when health container is deactivated
            }
        }

        // If not chasing, don't perform movement or shooting logic
        if (!isChasing)
            return;

        // Perform chasing logic only if chasing flag is true
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

        // Shoot at the player if time allows and if the enemy is still shooting
        if (timeBtwShots <= 0 && isShooting)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
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
        Debug.Log("Enemy is dying");
        // Check if the enemy is already dying
        if (!isDying && health <= 0)
        {
            foreach (LootItem lootItem in lootTable)
            {
                if (Random.Range(0f, 100f) <= lootItem.dropChance)
                {
                    InstantiateLoot(lootItem.itemPrefab);
                }
            }

            // Set the flag to true to indicate that the enemy is dying
            isDying = true;
            isShooting = false; // Stop shooting when dying

            // Trigger death animation if available
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                // Set the "IsDead" parameter to true in the Animator Controller
                animator.SetBool("IsDead", true);

                // Adjust the animation speed
                animator.speed = 0.5f; // Adjust this value as needed to slow down the animation
            }

            // Destroy the game object after the animation duration
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
            droppedLoot.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        Debug.Log("DestroyAfterAnimation coroutine started");
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

        Debug.Log("Destroying enemy object");
        // Animation has finished playing, destroy the game object
        Destroy(gameObject);

        yield return null;
    }
}
