using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyControl : MonoBehaviour
{

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float health = 50f;
    public float dropChancePercentage = 100f;
    public float flashDuration = 1.5f; // Duration of the flash effect
    private SpriteRenderer spriteRenderer;
    
    public GameObject fireflyHealthContainer;
    public GameObject itemPrefab1;
    public GameObject itemPrefab2;

    public float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        timeBtwShots = startTimeBtwShots;

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        }else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        } else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }


        if(timeBtwShots <= 0)
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

           

            // Destroy the game object after the animation duration
            StartCoroutine(DestroyAfterAnimation());
        }
    }





    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1f);// Randomly determine if the item prefabs should drop
        bool shouldDropItem1 = Random.Range(0f, 100f) < dropChancePercentage;
        bool shouldDropItem2 = Random.Range(0f, 100f) < dropChancePercentage;

        if (shouldDropItem1 && itemPrefab1 != null)
        {
            Instantiate(itemPrefab1, transform.position, Quaternion.identity);
            Debug.Log("Item 1 dropped");
        }
        if (shouldDropItem2 && itemPrefab2 != null)
        {
            Instantiate(itemPrefab2, transform.position, Quaternion.identity);
            Debug.Log("Item 2 dropped");
        }
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
