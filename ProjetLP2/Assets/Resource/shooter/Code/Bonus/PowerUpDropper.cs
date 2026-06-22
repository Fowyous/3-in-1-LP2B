using UnityEngine;
using System.Collections.Generic;

///<summary>
///Add this script to any enemy prefab to give it a chance to drop a power-up on death.
///Each enemy type can have its own drop chances and allowed power-up pool,
///configured directly in the Inspector.
///
///USAGE: Call TryDropPowerUp() from the enemy's death logic (TakeDamage when health <= 0),
///right before Destroy(gameObject) — the same way DeathExplosion or ScoreManager are called.
///</summary>
public class PowerUpDropper : MonoBehaviour
{
    [System.Serializable]
    public class PowerUpEntry
    {
        public PowerUpType type;
        public GameObject pickupPrefab;
        [Tooltip("Relative weight among the OTHER power-ups in this list (used only if a drop occurs).")]
        public float weight = 1f;
    }

    [Header("Drop Settings")]
    [Tooltip("Chance (0 to 1) that this enemy drops ANY power-up at all when it dies.")]
    [SerializeField] [Range(0f, 1f)] private float dropChance = 0.15f;

    [Tooltip("Pool of power-ups this enemy can drop, each with its own relative weight.")]
    [SerializeField] private List<PowerUpEntry> possibleDrops = new List<PowerUpEntry>();

    ///<summary>
    ///Rolls the dice for a drop, and if successful, picks a weighted random power-up
    ///from this enemy's pool and instantiates it at the enemy's current position.
    ///Call this right before the enemy's GameObject is destroyed.
    ///</summary>
    public void TryDropPowerUp()
    {
        if (possibleDrops.Count == 0) return;

        // First roll: does this enemy drop anything at all?
        if (Random.value > dropChance) return;

        // Second roll: which power-up from the pool?
        PowerUpEntry chosen = GetWeightedRandomDrop();
        if (chosen == null || chosen.pickupPrefab == null) return;

        Instantiate(chosen.pickupPrefab, transform.position, Quaternion.identity);
        Debug.Log($"PowerUpDropper: {gameObject.name} dropped a {chosen.type} power-up.");
    }

    private PowerUpEntry GetWeightedRandomDrop()
    {
        float totalWeight = 0f;
        foreach (var entry in possibleDrops) totalWeight += entry.weight;

        if (totalWeight <= 0f) return null;

        float randomValue = Random.Range(0f, totalWeight);
        foreach (var entry in possibleDrops)
        {
            randomValue -= entry.weight;
            if (randomValue <= 0f) return entry;
        }

        return possibleDrops[possibleDrops.Count - 1];
    }
}