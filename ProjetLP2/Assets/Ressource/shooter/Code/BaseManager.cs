using UnityEngine;
using System.Collections;

public class BaseManager : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float maxHealth = 300f;

    [Header("Regeneration Settings")]
    [SerializeField] private float regenAmountPerSecond = 5f;
    [SerializeField] private float regenDelay = 5f;

    public float CurrentHealth { get; private set; }

    private float timeOfLastDamage;

    ///<summary>
    ///Initializes the base health and sets up the timestamp.
    ///</summary>
    void Start()
    {
        CurrentHealth = maxHealth;
        timeOfLastDamage = -regenDelay;
    }

    ///<summary>
    ///Monitors the time since last damage to trigger passive regeneration.
    ///</summary>
    void Update()
    {
        if (Time.time - timeOfLastDamage >= regenDelay && CurrentHealth < maxHealth)
        {
            ApplyPassiveRegeneration();
        }
    }

    ///<summary>
    ///Heals the base gradually over time, capping at max health.
    ///</summary>
    private void ApplyPassiveRegeneration()
    {
        CurrentHealth += regenAmountPerSecond * Time.deltaTime;
        CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);
        Debug.Log($"Base regenerating... Current Health: {CurrentHealth:F1}/{maxHealth}");
    }

    ///<summary>
    ///Inflicts damage to the base and resets the regeneration timer.
    ///</summary>
    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        timeOfLastDamage = Time.time;

        Debug.Log($"Base attacked! Took {damageAmount} damage. Health remaining: {CurrentHealth:F1}/{maxHealth}");

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            TriggerGameOver();
        }
    }

    ///<summary>
    ///Handles the destruction of the base:
    ///- Disables future UFO respawns via RespawnManager
    ///- Triggers Game Over only if the UFO is also dead
    ///</summary>
    private void TriggerGameOver()
    {
        Debug.LogError("Base Destroyed!");

        // Disable respawns: from now on, if the UFO dies it's Game Over
        if (RespawnManager.Instance != null)
        {
            RespawnManager.Instance.CanRespawn = false;
            Debug.Log("BaseManager: Respawn disabled. Player must survive to avoid Game Over.");
        }
        else
        {
            Debug.LogWarning("BaseManager: RespawnManager not found, triggering Game Over directly.");
            if (GameOverManager.Instance != null)
                GameOverManager.Instance.TriggerGameOver();
        }
    }

    ///<summary>
    ///Detects enemies entering the base collider (Zone 3 wall boundary).
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if (enemy != null)
        {
            float totalDamage = enemy.Health + enemy.Damage;
            TakeDamage(totalDamage);
            Destroy(collision.gameObject);
        }
    }
}