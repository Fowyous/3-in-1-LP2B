using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static ShooterConstants;

public class EnemySpawner : MonoBehaviour
{
    ///<summary>
    ///Associates an enemy prefab with a spawn weight.
    ///Higher weight = appears more often.
    ///Configure directly in the Unity Inspector.
    ///</summary>
    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject prefab;
        [Tooltip("Higher = more frequent. Ex: Kamikaz=10, Boss=1")]
        public float weight = 1f;
    }

    [Header("Spawn Settings")]
    [SerializeField] private List<EnemyEntry> enemyEntries = new List<EnemyEntry>();
    [SerializeField] private float spawnInterval = 3f; // Delay in seconds between each spawn

    [Header("References")]
    public GameObject player;

    ///<summary>
    ///Validates that all entries implement IEnemy and have a positive weight.
    ///</summary>
    private void OnValidate()
    {
        for (int i = enemyEntries.Count - 1; i >= 0; i--)
        {
            EnemyEntry entry = enemyEntries[i];
            if (entry.prefab == null) continue;

            if (entry.prefab.GetComponent<IEnemy>() == null)
            {
                Debug.LogWarning($"'{entry.prefab.name}' doesn't implement IEnemy. Removed from list.");
                enemyEntries.RemoveAt(i);
                continue;
            }

            if (entry.weight <= 0f)
            {
                Debug.LogWarning($"'{entry.prefab.name}' has a weight of {entry.weight}. It should be > 0.");
                entry.weight = 1f;
            }
        }
    }

    ///<summary>
    ///Finds the player if not assigned, then starts the spawn loop.
    ///</summary>
    void Start()
    {
        if (player == null)
        {
            UFO foundPlayer = Object.FindAnyObjectByType<UFO>();
            if (foundPlayer != null)
            {
                player = foundPlayer.gameObject;
            }
            else
            {
                Debug.LogError("EnemySpawner: Player Target (UFO) is missing from the scene!");
                return;
            }
        }

        StartCoroutine(SpawnLoop());
    }

    ///<summary>
    ///Infinite coroutine that spawns a weighted random enemy every spawnInterval seconds.
    ///</summary>
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    ///<summary>
    ///Picks a weighted random enemy, instantiates it at the spawn position, and injects the player target.
    ///</summary>
    private void SpawnRandomEnemy()
    {
        if (enemyEntries == null || enemyEntries.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: No enemies configured in the list!");
            return;
        }

        EnemyEntry selectedEntry = GetWeightedRandomEnemy();
        if (selectedEntry == null || selectedEntry.prefab == null) return;

        // Spawn at the right edge of the screen, at a random Y position within game bounds
        Vector3 spawnPosition = new Vector3(
            ShooterConstants.GameLimit.x + 2f,
            Random.Range(-ShooterConstants.GameLimit.y, ShooterConstants.GameLimit.y),
            0f
        );

        GameObject newEnemy = Instantiate(selectedEntry.prefab, spawnPosition, Quaternion.identity);

        // Inject the player reference into the enemy AI
        IEnemy enemyScript = newEnemy.GetComponent<IEnemy>();
        if (enemyScript != null)
        {
            enemyScript.Target = player;
        }

        Debug.Log($"EnemySpawner: Spawned '{selectedEntry.prefab.name}' at Y={spawnPosition.y:F2}");
    }

    ///<summary>
    ///Selects an enemy entry using weighted random selection.
    ///The probability of each entry is proportional to its weight relative to the total weight sum.
    ///</summary>
    private EnemyEntry GetWeightedRandomEnemy()
    {
        // Step 1: Calculate the total sum of all weights
        float totalWeight = 0f;
        foreach (EnemyEntry entry in enemyEntries)
        {
            if (entry.prefab != null) totalWeight += entry.weight;
        }

        if (totalWeight <= 0f)
        {
            Debug.LogError("EnemySpawner: Total weight is zero, cannot select an enemy.");
            return null;
        }

        // Step 2: Pick a random value between 0 and the total weight
        float randomValue = Random.Range(0f, totalWeight);

        // Step 3: Walk through the list and subtract each weight until we reach 0
        foreach (EnemyEntry entry in enemyEntries)
        {
            if (entry.prefab == null) continue;
            randomValue -= entry.weight;
            if (randomValue <= 0f) return entry;
        }

        // Fallback: return the last entry (should never happen)
        return enemyEntries[enemyEntries.Count - 1];
    }
}