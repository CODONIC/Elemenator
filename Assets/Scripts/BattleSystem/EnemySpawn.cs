using UnityEngine;
using System.Collections;
public class EnemySpawn : MonoBehaviour
{
    public GameObject spawnEffectPrefab; // Prefab of the spawn effect

    void Awake()
    {
        Debug.Log("Awake: Disabling GameObject");
        gameObject.SetActive(false); // Disable the GameObject at the start
    }

    void Start()
    {
        Debug.Log("Start: Calling Spawn");
        Spawn();
    }

    public void Spawn()
    {
        StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // Small delay to ensure proper initialization

        Debug.Log("SpawnWithDelay: Instantiating spawn effect");
        // Instantiate the spawn effect
        GameObject spawnEffect = Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity);
        // Optionally, destroy the spawn effect after a short delay to clean up
        Destroy(spawnEffect, 2f); // Adjust the delay as needed

        Debug.Log("SpawnWithDelay: Enabling GameObject");
        // Enable the enemy GameObject to make it appear as if it has spawned
        gameObject.SetActive(true);
    }
}
