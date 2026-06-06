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
    private bool isRegenerating = false;

    ///<summary>
    ///Initializes the base health and sets up the timestamp.
    ///</summary>
    void Start()
    {
        CurrentHealth = maxHealth;
        timeOfLastDamage = -regenDelay; // Allows instant regen potential at start
    }

    ///<summary>
    ///Monitors the time since last damage to trigger passive regeneration.
    ///</summary>
    void Update()
    {
        // Check if 5 seconds have passed since the last hit, and if health is not already full
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
        CurrentHealth = Mathf.Min(CurrentHealth, maxHealth); // Clamp to maxHealth
        Debug.Log($"Base regenerating... Current Health: {CurrentHealth:F1}/{maxHealth}");
    }

    ///<summary>
    ///Inflicts damage to the base and resets the regeneration timer.
    ///</summary>
    ///<param name="damageAmount">The amount of damage to deduct from the base health.</param>
    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        timeOfLastDamage = Time.time; // Reset the 5-second cooldown timer
        
        Debug.Log($"Base attacked! Took {damageAmount} damage. Health remaining: {CurrentHealth:F1}/{maxHealth}");

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            TriggerGameOver();
        }
    }

    ///<summary>
    ///Handles the destruction of the base and triggers the end of the game.
    ///</summary>
    private void TriggerGameOver()
    {
        Debug.LogError("Base Destroyed! GAME OVER.");
        // TODO: Redirect to the Game Over scene as required by the specifications
    }

    ///<summary>
    ///Detects enemies entering the base collider (Zone 3 wall boundary).
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if (enemy != null)
        {
            // Calculate damage: remaining enemy health + full attack value (as specified in Zone 3 requirements)
            float totalDamage = enemy.Health + enemy.Damage;
            
            TakeDamage(totalDamage);

            // Destroy the enemy GameObject since it has successfully crashed into the wall
            Destroy(collision.gameObject);
        }
    }
}